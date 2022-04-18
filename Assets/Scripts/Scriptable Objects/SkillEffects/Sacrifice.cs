using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Sacrifice", menuName = "ScriptableObject/SkillEffect/Sacrifice")]
    public class Sacrifice : SkillEffect {
        private float HPPercentageSacrificed => value1;
        private bool HealGoesToAllies => (int)value2 % 2 == 0;
        private bool HealGoesToEnemies => (int)value2 % 2 == 1;
        public override string Description => $"Sacrifice {HPPercentageSacrificed * 100}% max HP of target and heal all {HealTargetsString} for that amount in total";

        private string HealTargetsString => HealGoesToAllies ? "allies" : "enemies";
        public override void Apply(BattleUnit source, BattleUnit target) {
            int damage = target.DecreaseHP(HPPercentageSacrificed);
            if (HealGoesToAllies) {
                DistributeHPToAllies(target, damage);
            } else if (HealGoesToEnemies) {
                DistributeHPToEnemies(target, damage);
            }
        }

        private static void DistributeHPToAllies(BattleUnit target, int damage) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            int healValue = (allies.Count > 1) ? damage / (allies.Count - 1) : 0;
            foreach (BattleUnit unit in allies) {
                if (unit != target)
                    unit.Heal(healValue, 1.0f, target);
            }
        }

        private static void DistributeHPToEnemies(BattleUnit target, int damage) {
            List<BattleUnit> enemies = BattleManager.instance.GetEnemies(target);
            int healValue = (enemies.Count > 0) ? damage / enemies.Count : 0;
            foreach (BattleUnit unit in enemies) {
                unit.Heal(healValue, 1.0f, target);
            }
        }
    }
}
