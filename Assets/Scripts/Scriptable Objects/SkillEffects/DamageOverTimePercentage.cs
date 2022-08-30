using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DamageOverTimePercentage", menuName = "ScriptableObject/SkillEffect/DamageOverTimePercentage")]
    public class DamageOverTimePercentage : SkillEffect {
        private float MaxHPMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {MaxHPMultiplier * 100}% of Target's Max HP as Neutral damage each turn for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new LosingHPOverTime(source, target, MaxHPMultiplier, Element.Neutral, DoTDuration));
        }
    }
}