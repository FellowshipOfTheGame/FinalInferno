using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Taunt", menuName = "ScriptableObject/SkillEffect/Taunt")]
    public class Taunt : SkillEffect {
        // value1 = Aggro gain per turn
        // value2 = status duration
        public override string Description => "Gain " + value1 + " aggro for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Taunting(source, target, value1, (int)value2));
        }
    }
}