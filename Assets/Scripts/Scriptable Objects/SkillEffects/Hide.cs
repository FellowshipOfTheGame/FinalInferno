using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Hide", menuName = "ScriptableObject/SkillEffect/Hide")]
    public class Hide : SkillEffect {
        private float NegativeAggroPerTurn => value1;
        private int Duration => (int)value2;
        public override string Description => $"Decrease source's aggro by {NegativeAggroPerTurn} every turn for {Duration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            source.AddEffect(new Hiding(source, source, NegativeAggroPerTurn, Duration));
        }
    }
}