using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Burn", menuName = "ScriptableObject/SkillEffect/Burn")]
    public class Burn : SkillEffect {
        // value1 = dmg multiplier
        // value2 = DoT duration
        public override string Description => "Deals " + value1 + "x Fire damage for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Fire, (int)value2));
        }
    }
}