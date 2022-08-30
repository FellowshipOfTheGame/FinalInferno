using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Quicksand", menuName = "ScriptableObject/SkillEffect/Quicksand")]
    public class Quicksand : SkillEffect {
        private float DmgMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {DmgMultiplier}x Earth damage for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, DmgMultiplier, Element.Earth, DoTDuration));
        }
    }
}