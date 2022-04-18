using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreaseDefense", menuName = "ScriptableObject/SkillEffect/DecreaseDefense")]
    public class DecreaseDefense : SkillEffect {
        private float DefDownMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Decrease defense by {DefDownMultiplier * 100}% for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (DebuffDuration < 0) {
                target.curDef -= (int)(DefDownMultiplier * target.curDef);
            } else {
                target.AddEffect(new DefenseDown(source, target, DefDownMultiplier, DebuffDuration));
            }
        }
    }
}