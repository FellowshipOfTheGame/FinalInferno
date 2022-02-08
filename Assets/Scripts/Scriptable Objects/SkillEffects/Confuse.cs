using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Confuse", menuName = "ScriptableObject/SkillEffect/Confuse")]
    public class Confuse : SkillEffect {
        // value1 = chance to confuse
        // value2 = duration
        public override string Description => "Confuse with " + value1 * 100 + "% of chance for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Confused(source, target, value1, (int)value2));
        }
    }
}