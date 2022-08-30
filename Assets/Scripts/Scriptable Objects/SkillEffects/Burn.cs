using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Burn", menuName = "ScriptableObject/SkillEffect/Burn")]
    public class Burn : SkillEffect {
        private float DmgMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {DmgMultiplier}x Fire damage for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, DmgMultiplier, Element.Fire, DoTDuration));
        }
    }
}