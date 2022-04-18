using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreaseDefense", menuName = "ScriptableObject/SkillEffect/IncreaseDefense")]
    public class IncreaseDefense : SkillEffect {
        private float DefUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase defense by {DefUpMultiplier * 100}% for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (BuffDuration < 0) {
                target.curDef += (int)(DefUpMultiplier * target.curDef);
            } else {
                target.AddEffect(new DefenseUp(source, target, DefUpMultiplier, BuffDuration));
            }
        }
    }
}