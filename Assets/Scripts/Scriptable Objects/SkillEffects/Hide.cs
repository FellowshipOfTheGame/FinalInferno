using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Hide", menuName = "ScriptableObject/SkillEffect/Hide")]
    public class Hide : SkillEffect {
        // value1 = Negative aggro each turn
        // value2 = status duration
        public override string Description => "Decrease source's aggro by " + value1 + " for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            source.AddEffect(new Hiding(source, source, value1, (int)value2));
        }
    }
}