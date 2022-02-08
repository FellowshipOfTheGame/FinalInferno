using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DamageDrain", menuName = "ScriptableObject/SkillEffect/DamageDrain")]
    public class DamageDrain : SkillEffect {
        // value1 = dmgDrain multiplier
        // value2 = debuff duration
        public override string Description => "Drain " + value1 * 100 + "% damage for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            bool isDraining = false;
            foreach (StatusEffect effect in target.effects) {
                if (effect.GetType() == typeof(DamageDrained) && effect.Source == source) {
                    isDraining = true;
                    break;
                }
            }
            if (!isDraining) {
                source.AddEffect(new DrainingDamage(source, target, value1, (int)value2));
                target.AddEffect(new DamageDrained(source, target, value1, (int)value2));
            }
        }
    }
}