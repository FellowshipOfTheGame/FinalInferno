using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Morph", menuName = "ScriptableObject/SkillEffect/Morph")]
    public class Morph : SkillEffect {
        [SerializeField] private UnitIndex unitIndex;
        private float PercentageMaxHealth => Mathf.Clamp(value2, 0, 1f);
        private int MorphUnitIndex => HasValidUnitIndexList ? Mathf.Clamp((int)value1, 0, unitIndex.UnitList.Count - 1) : -1;
        private bool HasValidUnitIndexList => unitIndex != null && unitIndex.UnitList != null && unitIndex.UnitList.Count > 0;
        public override string Description {
            get {
                if (!HasValidUnitIndexList) {
                    Debug.LogError($"SkillEffect Morph can't access UnitIndex list", this);
                    return "ERROR";
                }

                string targetUnit = unitIndex.UnitList[MorphUnitIndex].name;
                return $"Morph into {targetUnit} with {PercentageMaxHealth * 100}% of Max HP";
            }
        }


        public override void Apply(BattleUnit source, BattleUnit target) {
            if (!HasValidUnitIndexList) {
                Debug.LogError($"SkillEffect Morph can't access UnitIndex list", this);
                return;
            }

            int previousHP = target.CurHP;
            LevelUpIfEnemy();
            ConfigureStatsAndUI(target);
            ShowHPChange(target, previousHP);
        }

        private void LevelUpIfEnemy() {
            Enemy enemy = unitIndex.UnitList[MorphUnitIndex] as Enemy;
            if (enemy)
                enemy.LevelEnemy();
        }

        private void ConfigureStatsAndUI(BattleUnit target) {
            target.ConfigureMorph(unitIndex.UnitList[MorphUnitIndex], PercentageMaxHealth);
            target.OnSizeChanged?.Invoke();
        }

        private static void ShowHPChange(BattleUnit target, int previousHP) {
            int hpDifference = target.CurHP - previousHP;
            if (hpDifference != 0) {
                target.ShowDamage(Mathf.Abs(hpDifference), hpDifference > 0, 1.0f);
            }
        }
    }
}
