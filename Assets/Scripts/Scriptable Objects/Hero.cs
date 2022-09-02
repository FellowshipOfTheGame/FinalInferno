using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObject/Hero", order = 2)]
    public class Hero : Unit, IDatabaseItem {
        public Sprite spriteOW;
        public RuntimeAnimatorController animatorOW;
        [Space(10)]
        [Header("Hero Info")]
        [SerializeField] private List<PlayerSkill> initialsSkills = new List<PlayerSkill>();
        public List<PlayerSkill> skillsToUpdate;
        private long DefaultSkillExp => Mathf.Max(10, (Mathf.FloorToInt(Mathf.Sqrt(Party.Instance.XpCumulative))));
        public override long SkillExp {
            get {
                if (!BattleManager.instance)
                    return DefaultSkillExp;
                long expSum = 0;
                int nEnemies = 0;
                foreach (Unit unit in BattleManager.instance.Units) {
                    if (!unit.IsHero) {
                        expSum += unit.SkillExp;
                        nEnemies++;
                    }
                }
                return (nEnemies > 0) ? expSum / nEnemies : DefaultSkillExp;
            }
        }
        [Space(10)]
        [Header("Table")]
        [SerializeField] private TextAsset heroTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                table ??= DynamicTable.Create(heroTable);
                return table;
            }
        }
        public override bool IsHero => true;
        public override UnitType UnitType => UnitType.Hero;

        #region IDatabaseItem
        public void LoadTables() {
            table = DynamicTable.Create(heroTable);
        }

        public void Preload() {
            elementalResistances.Clear();
            skillsToUpdate = new List<PlayerSkill>(initialsSkills);
            LevelUpIgnoringSkills(-1);
        }
        #endregion

        private void LevelUpIgnoringSkills(int newLevel) {
            level = Mathf.Clamp(newLevel, 1, Table.Rows.Count);
            LoadStatsFromTable();
        }

        private void LoadStatsFromTable() {
            int tableIndex = level - 1;
            hpMax = Table.Rows[tableIndex].Field<int>(HPColumnName);
            baseDmg = Table.Rows[tableIndex].Field<int>(DamageColumnName);
            baseDef = Table.Rows[tableIndex].Field<int>(DefenseColumnName);
            baseMagicDef = Table.Rows[tableIndex].Field<int>(ResistanceColumnName);
            baseSpeed = Table.Rows[tableIndex].Field<int>(SpeedColumnName);
        }

        public void LevelUp(int newLevel) {
            LevelUpIgnoringSkills(newLevel);
            UnlockSkills();
        }

        public void UnlockSkills() {
            foreach (PlayerSkill skill in skillsToUpdate.ToArray()) {
                if (skill.CheckUnlock(level))
                    UpdateUnlockedSkillStatus(skill);
            }
        }

        private void UpdateUnlockedSkillStatus(PlayerSkill unlockedSkill) {
            foreach (PlayerSkill child in unlockedSkill.skillsToUpdate) {
                if (!skillsToUpdate.Contains(child))
                    skillsToUpdate.Add(child);
            }
            skillsToUpdate.Remove(unlockedSkill);
        }

        public void ResetHero() {
            elementalResistances.Clear();
            foreach (Skill skill in skills) {
                skill.ResetSkill();
            }
            skillsToUpdate = new List<PlayerSkill>(initialsSkills);
        }

        public override Color DialogueColor => color;
        public override string DialogueName => name ?? "";
    }
}
