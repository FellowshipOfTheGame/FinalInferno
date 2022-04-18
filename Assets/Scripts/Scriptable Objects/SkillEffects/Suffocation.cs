using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Suffocation", menuName = "ScriptableObject/SkillEffect/Suffocation")]
    public class Suffocation : SkillEffect {
        private float DmgMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {DmgMultiplier}x Wind damage for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, DmgMultiplier, Element.Wind, DoTDuration));
        }
    }
}