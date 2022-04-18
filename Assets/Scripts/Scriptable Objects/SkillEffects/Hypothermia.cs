using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Hypothermia", menuName = "ScriptableObject/SkillEffect/Hypothermia")]
    public class Hypothermia : SkillEffect {
        private float DmgMultiplier => value1;
        private int DoTDuration => (int)value2;
        public override string Description => $"Deals {DmgMultiplier}x Water damage for {DoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, DmgMultiplier, Element.Water, DoTDuration));
        }
    }
}