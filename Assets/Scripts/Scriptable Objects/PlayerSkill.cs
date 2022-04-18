using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
    public class PlayerSkill : Skill {
        protected const string levelColumnString = "Level";
        protected const string accumulatedXpColumnName = "XPAccumulated";
        protected const string xpNextLevelColumnName = "XPNextLevel";
        [Header("Player Skill")]
        public long xp;
        public long xpNext;
        public long XpCumulative => (table == null) ? 0 : (xp + CurrentLevelCumulativeXp);
        private long CurrentLevelCumulativeXp => (level <= 1) ? 0 : XpTable.Rows[level - 2].Field<long>(accumulatedXpColumnName);
        private bool ShouldIncreaseLevel => xp >= xpNext && level < XpTable.Rows.Count && level < Table.Rows.Count;
        public int MinLevel => Mathf.Max(XpTable.Rows[0].Field<int>(levelColumnString), Table.Rows[0].Field<int>(levelColumnString));
        public int MaxLevel => Mathf.Min(XpTable.Rows[XpTable.Rows.Count - 1].Field<int>(levelColumnString), Table.Rows[Table.Rows.Count - 1].Field<int>(levelColumnString));
        [TextArea] public string description;
        public override string ShortDescription => (!string.IsNullOrEmpty(shortDescription)) ? shortDescription : description;
        [Header("Unlock Info")]
        public List<PlayerSkill> skillsToUpdate;
        public List<PlayerSkill> prerequisiteSkills;
        public List<int> prerequisiteSkillsLevel;
        public int prerequisiteHeroLevel;
        [Header("Stats Table")]
        [SerializeField] private TextAsset skillTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                table ??= DynamicTable.Create(skillTable);
                return table;
            }
        }
        [Header("Exp Table")]
        [SerializeField] private TextAsset expTable;
        [SerializeField] private DynamicTable xpTable;
        private DynamicTable XpTable {
            get {
                xpTable ??= DynamicTable.Create(expTable);
                return xpTable;
            }
        }
        public Sprite skillImage;

        #region IDatabaseItem
        public override void LoadTables() {
            table = DynamicTable.Create(skillTable);
            xpTable = DynamicTable.Create(expTable);
        }

        public override void Preload() {
            level = 0;
            xp = 0;
            xpNext = 0;
        }
        #endregion

        public void GiveExp(List<BattleUnit> targets) {
            long expValue = CalculateExpFromBattleUnits(targets);
            GiveExp(expValue);
        }

        private static long CalculateExpFromBattleUnits(List<BattleUnit> targets) {
            long expValue = 0;
            foreach (BattleUnit target in targets) {
                expValue += target.Unit.SkillExp;
            }
            expValue /= Mathf.Max(targets.Count, 1);
            return expValue;
        }

        public void GiveExp(long value) {
            xp += value;
            bool leveledUp = ShouldIncreaseLevel;
            while (ShouldIncreaseLevel) {
                LevelUp();
            }
            if (leveledUp)
                UpdateToCurrentLevel();
        }

        private void LevelUp() {
            xp -= xpNext;
            level++;
            xpNext = XpTable.Rows[level - 1].Field<long>(xpNextLevelColumnName);
        }

        public void UpdateToCurrentLevel() {
            for (int skillEffectIndex = 0; skillEffectIndex < effects.Count; skillEffectIndex++) {
                UpdateSkillEffectValues(skillEffectIndex);
            }
            foreach (PlayerSkill child in skillsToUpdate) {
                child.CheckUnlock(Party.Instance.Level);
            }
        }

        private void UpdateSkillEffectValues(int skillEffectIndex) {
            SkillEffectTuple modifyEffect = effects[skillEffectIndex];
            modifyEffect.value1 = Table.Rows[level - 1].Field<float>(SkillEffectValueString(skillEffectIndex, 0));
            modifyEffect.value2 = Table.Rows[level - 1].Field<float>(SkillEffectValueString(skillEffectIndex, 1));
            effects[skillEffectIndex] = modifyEffect;
        }

        private static string SkillEffectValueString(int skillEffectIndex, int valueIndex) {
            return $"SkillEffect{skillEffectIndex}Value{valueIndex}";
        }

        public void GiveExp(List<Unit> units) {
            long expValue = CalculateExpFromUnits(units);
            GiveExp(expValue);
        }

        private static long CalculateExpFromUnits(List<Unit> units) {
            long expValue = 0;
            foreach (Unit unit in units) {
                expValue += unit.SkillExp;
            }
            expValue /= Mathf.Max(units.Count, 1);
            return expValue;
        }

        public bool CheckUnlock(int heroLevel) {
            if (level > 0)
                return true;

            bool levelCheck = (heroLevel >= prerequisiteHeroLevel);
            if (!levelCheck)
                return false;

            bool prerequisitesSatisfied = CheckAllPrerequisites();
            if (prerequisitesSatisfied)
                UnlockSkill();
            return prerequisitesSatisfied;
        }

        private bool CheckAllPrerequisites() {
            bool prerequisitesSatisfied = true;
            for (int i = 0; i < prerequisiteSkills.Count; i++) {
                prerequisitesSatisfied &= (prerequisiteSkills[i].Level >= prerequisiteSkillsLevel[i]);
            }
            return prerequisitesSatisfied;
        }

        private void UnlockSkill() {
            GiveExp(0);
            active = true;
        }

        #region Overrides
        public override void UseCallbackOrDelayed(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            targets = FilterTargets(user, targets);
            GiveExpOnCallbackOrDelayed(user, targets);
            base.UseCallbackOrDelayed(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }

        private void GiveExpOnCallbackOrDelayed(BattleUnit user, List<BattleUnit> targets) {
            if (Type == SkillType.PassiveOnStart || Type == SkillType.PassiveOnEnd) {
                if (PassiveSkillShouldUseExpMean()) {
                    GiveExp(BattleManager.instance.GetEnemies(user, true));
                } else {
                    GiveExp(targets);
                }
            } else {
                GiveExp(targets);
            }
        }

        private bool PassiveSkillShouldUseExpMean() {
            return target != TargetType.AllAlliesLiveOrDead && target != TargetType.AllEnemiesLiveOrDead &&
                   target != TargetType.AllDeadAllies && target != TargetType.AllDeadEnemies &&
                   target != TargetType.AllLiveAllies && target != TargetType.AllLiveEnemies;
        }

        public override void ResetSkill() {
            level = 0;
            xp = 0;
            xpNext = 0;
            active = false;
        }
        #endregion
    }
}
