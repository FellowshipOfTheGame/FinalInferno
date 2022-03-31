using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EnemySkill", menuName = "ScriptableObject/EnemySkill")]
    public class EnemySkill : Skill {
        protected const string levelColumnName = "Level";
        [Header("Enemy Skill")]
        public string description;
        [Header("Stats Table")]
        [SerializeField] private TextAsset skillTable;
        [SerializeField] private DynamicTable table;
        private DynamicTable Table {
            get {
                if (table == null && skillTable != null) {
                    table = DynamicTable.Create(skillTable);
                } else if (skillTable == null) {
                    table = null;
                }

                return table;
            }
        }
        protected int MinLevel => Table.Rows[0].Field<int>(levelColumnName);
        protected int MaxLevel => Table.Rows[Table.Rows.Count-1].Field<int>(levelColumnName);
        public override int Level {
            get => level;
            set {
                if (value != level && Table != null && Table.Rows.Count > 0) {
                    level = Mathf.Clamp(value, MinLevel, MaxLevel);
                    LevelUp();
                }
            }
        }
        private int curTableRow = 0;

		#region IDatabaseItem
        public override void LoadTables() {
            table = DynamicTable.Create(skillTable);
        }

        public override void Preload() {
            active = true;
            curTableRow = -1;
            Level = -1;
        }
        #endregion

        public void LevelUp() {
            if (Table == null || Table.Rows.Count < 1) {
                Debug.LogWarning($"This skill({name}) has no table to load", this);
                return;
            }

            int newRow = GetTableRow();
            if (newRow == curTableRow) {
                return;
            }

            curTableRow = newRow;
            for (int skillEffectIndex = 0; skillEffectIndex < effects.Count; skillEffectIndex++) {
                UpdateSkillEffectValues(skillEffectIndex);
            }
        }

        private int GetTableRow() {
            int row = -1;
            do {
                row++;
            } while (row < Table.Rows.Count-1 && Table.Rows[row+1].Field<int>(levelColumnName) <= Level);
            return row;
        }

        private void UpdateSkillEffectValues(int skillEffectIndex) {
            SkillEffectTuple modifyEffect = effects[skillEffectIndex];
            modifyEffect.value1 = Table.Rows[curTableRow].Field<float>(SkillEffectValueString(skillEffectIndex, 0));
            modifyEffect.value2 = Table.Rows[curTableRow].Field<float>(SkillEffectValueString(skillEffectIndex, 1));
            effects[skillEffectIndex] = modifyEffect;
        }

        private static string SkillEffectValueString(int skillEffectIndex, int valueIndex) {
            return $"SkillEffect{skillEffectIndex}Value{valueIndex}";
        }

        public override void ResetSkill() {
            Level = 0;
            active = true;
        }

        public override void UseCallbackOrDelayed(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            targets = FilterTargets(user, targets);
            base.UseCallbackOrDelayed(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }
    }
}