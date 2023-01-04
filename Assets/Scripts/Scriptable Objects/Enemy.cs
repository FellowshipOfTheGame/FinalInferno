using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy/Basic")]
    public class Enemy : Unit, IDatabaseItem {
        private const string LevelSkillColumnName = "LevelSkill";
        private const string ColorColumnName = "Color";
        private const string LevelColumnName = "Level";
        private const string RankColumnName = "Rank";
        private const string BaseExpColumnName = "XP";
        public override UnitType UnitType => UnitType.Enemy;
        public Color dialogueColor;
        [Space(10)]
        [Header("Enemy Info")]
        [SerializeField] protected Element element = Element.Neutral;
        public Element Element => element;
        [SerializeField] protected DamageType damageFocus = DamageType.None;
        public DamageType DamageFocus => damageFocus;
        public override Color DialogueColor => dialogueColor;
        public override string DialogueName => AssetName ?? "";
        [SerializeField] private Sprite bestiaryPortrait;
        public Sprite BestiaryPortrait => bestiaryPortrait;
        [SerializeField] private AudioClip enemyCry;
        public AudioClip EnemyCry => enemyCry;
        [SerializeField, TextArea] private string bio = "Bio";
        public string Bio => bio;
        [Space(10)]
        [Header("Table")]
        [SerializeField] protected TextAsset enemyTable;
        [SerializeField] protected DynamicTable table;
        protected DynamicTable Table {
            get {
                if (table == null && enemyTable != null) {
                    table = DynamicTable.Create(enemyTable);
                } else if (enemyTable == null) {
                    table = null;
                }

                return table;
            }
        }
        [SerializeField, HideInInspector] protected int curTableRow = 0;
        protected int MinLevel => Table.Rows[0].Field<int>(LevelColumnName);
        protected int MaxLevel => Table.Rows[Table.Rows.Count - 1].Field<int>(LevelColumnName);
        public override long SkillExp => BaseExp;
        public long BaseExp { get; protected set; }

        #region IDatabaseItem
        public void LoadTables() {
            table = DynamicTable.Create(enemyTable);
        }

        public void Preload() {
            curTableRow = -1;
            LevelEnemy(-1);
        }
        #endregion

        public int GetSkillLevel(EnemySkill skill) {
            int skillIndex = skills.IndexOf(skill);
            if (skillIndex < 0)
                return 0;
            return Table.Rows[curTableRow].Field<int>($"{LevelSkillColumnName}{skillIndex}");
        }

        public int LevelEnemy() {
            int enemyLevel = CalculateEnemyLevel();
            LevelEnemy(enemyLevel);
            return enemyLevel;
        }

        public static int CalculateEnemyLevel() {
            int scaledLevel = Party.Instance.ScaledLevel;
            int enemyTier = CalculateEnemyTier(scaledLevel);
            int enemyTierLevel = CalculateEnemyTierLevel(scaledLevel);
            return enemyTier + enemyTierLevel;
        }

        private static int CalculateEnemyTier(int scaledLevel) {
            int enemyTier = 10 * (scaledLevel / 10);
            if (scaledLevel == enemyTier && enemyTier >= 10)
                enemyTier -= 10;
            return enemyTier;
        }

        private static int CalculateEnemyTierLevel(int scaledLevel) {
            while (scaledLevel > 10) {
                scaledLevel -= 10;
            }
            if (scaledLevel > 5)
                return 5;
            return 0;
        }

        public void LevelEnemy(int newLevel) {
            if (Table == null || Table.Rows.Count < 1) {
                Debug.LogWarning($"This enemy({AssetName}) has no table to load", this);
                return;
            }

            level = Mathf.Clamp(newLevel, MinLevel, MaxLevel);
            int newTableRow = GetTableRowWithLevel(newLevel);
            if (newTableRow != curTableRow) {
                curTableRow = newTableRow;
                LoadStatsFromTable(newTableRow);
            }
            LoadSkillLevelsFromTable();
        }

        private int GetTableRowWithLevel(int newLevel) {
            int row = -1;
            do {
                row++;
            } while (row < Table.Rows.Count - 1 && Table.Rows[row + 1].Field<int>(LevelColumnName) <= newLevel);
            return row;
        }

        private void LoadStatsFromTable(int tableRow) {
            name = Table.Rows[tableRow].Field<string>(RankColumnName);
            level = Table.Rows[tableRow].Field<int>(LevelColumnName);
            hpMax = Table.Rows[tableRow].Field<int>(HPColumnName);
            baseDmg = Table.Rows[tableRow].Field<int>(DamageColumnName);
            baseDef = Table.Rows[tableRow].Field<int>(DefenseColumnName);
            baseMagicDef = Table.Rows[tableRow].Field<int>(ResistanceColumnName);
            baseSpeed = Table.Rows[tableRow].Field<int>(SpeedColumnName);
            BaseExp = Table.Rows[tableRow].Field<int>(BaseExpColumnName);
            color = Table.Rows[tableRow].Field<Color>(ColorColumnName);
            LoadElementalResistancesFromTable(tableRow);
        }

        private void LoadElementalResistancesFromTable(int tableRow) {
            elementalResistances.Clear();
            foreach (Element element in System.Enum.GetValues(typeof(Element))) {
                LoadElementalResistanceIfValid(tableRow, element);
            }
        }

        private void LoadElementalResistanceIfValid(int tableRow, Element element) {
            string colName = ElementalResistColumnName(element);
            if (!Table.Rows[tableRow].HasField<float>(colName))
                return;
            float value = Table.Rows[tableRow].Field<float>(colName);
            if (value != 1.0f)
                elementalResistances.Add(element, value);
        }

        private static string ElementalResistColumnName(Element element) {
            return $"{System.Enum.GetName(typeof(Element), element)}Resistance";
        }

        private void LoadSkillLevelsFromTable() {
            for (int i = 0; i < skills.Count; i++) {
                skills[i].Level = Table.Rows[curTableRow].Field<int>($"{LevelSkillColumnName}{i}");
            }
        }

        #region EnemyAI
        public virtual void AIEnemy() {
            float relativeHP = BattleManager.instance.CurrentUnit.CurHP / GetAverageTeamHP();
            float percentageNotDefense = Mathf.Sqrt(relativeHP) + 0.05f * relativeHP;
            Skill skill = SkillDecision(percentageNotDefense);
            BattleSkillManager.SelectSkill(skill);
            BattleSkillManager.SetTargets(GetTargets(skill.target));
        }

        private static float GetAverageTeamHP() {
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            float average = 0.0f;
            foreach (BattleUnit unit in team) {
                average += unit.CurHP;
            }
            average /= team.Count;
            return average;
        }

        protected virtual Skill SkillDecision(float percentageNotDefense) {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < percentageNotDefense)
                return AttackDecision();
            return defenseSkill;
        }

        public virtual Skill AttackDecision() {
            return attackSkill;
        }

        public virtual void ResetParameters() { /* Função para resetar parametros de boss por exemplo */ }

        protected virtual List<BattleUnit> GetTargets(TargetType type) {
            return type switch {
                TargetType.Self => new List<BattleUnit>() { BattleManager.instance.CurrentUnit },
                TargetType.AllLiveAllies => BattleManager.instance.GetTeam(UnitType.Enemy),
                TargetType.AllLiveEnemies => BattleManager.instance.GetTeam(UnitType.Hero),
                TargetType.SingleLiveAlly => new List<BattleUnit>() { GetRandomLiveAlly() },
                TargetType.SingleLiveEnemy => new List<BattleUnit>() { TargetDecision(GetHeroesTeam()) },
                _ => throw new System.NotImplementedException("[Enemy.cs]: Target type not implemented for enemy targeting")
            };
        }

        protected static List<BattleUnit> GetHeroesTeam() {
            return BattleManager.instance.GetTeam(UnitType.Hero);
        }

        protected static BattleUnit GetRandomLiveAlly() {
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            return team[Random.Range(0, team.Count - 1)];
        }

        protected virtual BattleUnit TargetDecision(List<BattleUnit> targetTeam) {
            List<float> targetingChances = GetUnitTargetingChances(targetTeam);
            float roll = Random.Range(0.0f, 1.0f);
            for (int targetIndex = 0; targetIndex < targetTeam.Count; targetIndex++) {
                if (roll <= targetingChances[targetIndex])
                    return targetTeam[targetIndex];
                roll -= targetingChances[targetIndex];
            }
            return targetTeam[0];
        }

        protected static List<float> GetUnitTargetingChances(List<BattleUnit> targetTeam) {
            float sumTotal = GetTeamTotalAggro(targetTeam);
            List<float> percentual = new List<float>();
            foreach (BattleUnit unit in targetTeam) {
                percentual.Add(Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue) / sumTotal);
            }
            return percentual;
        }

        protected static float GetTeamTotalAggro(List<BattleUnit> targetTeam) {
            float sumTotal = 0.0f;
            foreach (BattleUnit unit in targetTeam) {
                sumTotal += Mathf.Clamp(unit.aggro, Mathf.Epsilon, float.MaxValue);
            }
            return sumTotal;
        }

        protected static bool AllHeroesAreParalised() {
            foreach (BattleUnit hero in BattleManager.instance.GetTeam(UnitType.Hero)) {
                if (hero.CanAct)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
