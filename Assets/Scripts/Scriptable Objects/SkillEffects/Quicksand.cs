using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Quicksand", menuName = "ScriptableObject/SkillEffect/Quicksand")]
    public class Quicksand : SkillEffect {
        // value1 = dmg multiplier
        // value2 = DoT duration
        public override string Description => "Deals " + value1 + "x Earth damage for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Earth, (int)value2));
        }
    }
}