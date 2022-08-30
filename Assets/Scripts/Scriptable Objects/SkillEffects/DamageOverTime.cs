using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DamageOverTime", menuName = "ScriptableObject/SkillEffect/DamageOverTime")]
    public class DamageOverTime : SkillEffect {
        private float DmgMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {DmgMultiplier}x Neutral damage for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, DmgMultiplier, Element.Neutral, DoTDuration));
        }
    }
}