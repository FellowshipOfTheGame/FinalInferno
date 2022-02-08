using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreaseDefense", menuName = "ScriptableObject/SkillEffect/IncreaseDefense")]
    public class IncreaseDefense : SkillEffect {
        // value1 = defUp multiplier
        // value2 = buff duration
        public override string Description => "Increase defense by " + value1 * 100 + "% for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (value2 < 0) {
                target.curDef += (int)value1 * target.curDef;
            } else {
                target.AddEffect(new DefenseUp(source, target, value1, (int)value2));
            }
        }
    }
}