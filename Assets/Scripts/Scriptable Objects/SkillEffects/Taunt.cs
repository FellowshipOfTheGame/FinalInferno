using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Taunt", menuName = "ScriptableObject/SkillEffect/Taunt")]
    public class Taunt : SkillEffect {
        private float AggroGainedPerTurn => value1;
        private int Duration => (int)value2;
        public override string Description => $"Gain {AggroGainedPerTurn} aggro for {Duration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Taunting(source, target, AggroGainedPerTurn, Duration));
        }
    }
}