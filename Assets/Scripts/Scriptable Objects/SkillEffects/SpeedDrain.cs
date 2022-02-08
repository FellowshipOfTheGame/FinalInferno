using UnityEngine;


namespace FinalInferno {
    [CreateAssetMenu(fileName = "SpeedDrain", menuName = "ScriptableObject/SkillEffect/SpeedDrain")]
    public class SpeedDrain : SkillEffect {
        // value1 = speedDrain multiplier
        // value2 = debuff duration
        public override string Description => "Drain " + value1 * 100 + "% speed for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            bool isDraining = false;
            foreach (StatusEffect effect in target.effects) {
                if (effect.GetType() == typeof(SpeedDrained) && effect.Source == source) {
                    isDraining = true;
                    break;
                }
            }
            if (!isDraining) {
                source.AddEffect(new DrainingSpeed(source, target, value1, (int)value2));
                target.AddEffect(new SpeedDrained(source, target, value1, (int)value2));
            }
        }
    }
}
