using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreaseSpeed", menuName = "ScriptableObject/SkillEffect/DecreaseSpeed")]
    public class DecreaseSpeed : SkillEffect {
        private float SpeedDownMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Decrease speed by {SpeedDownMultiplier * 100}% for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (DebuffDuration < 0) {
                target.CurSpeed -= (int)(SpeedDownMultiplier * target.CurSpeed);
            } else {
                target.AddEffect(new SpeedDown(source, target, SpeedDownMultiplier, DebuffDuration));
            }
        }
    }
}