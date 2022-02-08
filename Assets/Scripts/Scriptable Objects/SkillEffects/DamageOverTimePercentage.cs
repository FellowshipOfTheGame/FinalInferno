using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DamageOverTimePercentage", menuName = "ScriptableObject/SkillEffect/DamageOverTimePercentage")]
    public class DamageOverTimePercentage : SkillEffect {
        // value1 = MaxHP multiplier
        // value2 = DoT duration
        public override string Description => "Deals " + (value1 * 100) + "% of Target's Max HP as Neutral damage each turn for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new LosingHPOverTime(source, target, value1, Element.Neutral, (int)value2));
        }
    }
}