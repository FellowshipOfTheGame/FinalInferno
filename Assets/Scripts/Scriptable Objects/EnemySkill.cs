using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    //engloba todas as "skills" dos personagens do jogador, que ganham nivel
    [CreateAssetMenu(fileName = "EnemySkill", menuName = "ScriptableObject/EnemySkill")]
    public class EnemySkill : Skill {
        [Header("Enemy Skill")]
        public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
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
        public override int Level {
            get => level;
            set {
                if (value != level && Table != null && Table.Rows.Count > 0) {
                    level = Mathf.Clamp(value, Table.Rows[0].Field<int>("Level"), Table.Rows[Table.Rows.Count - 1].Field<int>("Level"));
                    LevelUp();
                }
            }
        }
        private int curTableRow = 0;

        public override void LoadTables() {
            table = DynamicTable.Create(skillTable);
        }

        public override void Preload() {
            active = true;
            curTableRow = -1;
            Level = -1; // O valor é -1 para garantir que seja diferente do default 0
        }

        //atualiza o value dos efeitos, se for necessario.
        public void LevelUp() {
            if (Table == null || Table.Rows.Count < 1) {
                Debug.Log($"This skill({name}) has no table to load");
                return;
            }

            int row = -1;
            do {
                row++;
            } while (row < Table.Rows.Count - 1 && Table.Rows[row + 1].Field<int>("Level") <= Level);

            if (row != curTableRow) {
                curTableRow = row;
                for (int i = 0; i < effects.Count; i++) {
                    SkillEffectTuple modifyEffect = effects[i];

                    modifyEffect.value1 = Table.Rows[curTableRow].Field<float>("SkillEffect" + i + "Value0");
                    modifyEffect.value2 = Table.Rows[curTableRow].Field<float>("SkillEffect" + i + "Value1");

                    effects[i] = modifyEffect;
                }
            }
        }

        public override void ResetSkill() {
            Level = 0;
            active = true;
            // Debug.Log("Skill resetada");
        }

        public override void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f) {
            targets = FilterTargets(user, targets); // Filtragem para garantir a consistencia dos callbacks de AoE
            base.Use(user, targets, shouldOverride1, value1, shouldOverride2, value2);
        }
    }
}