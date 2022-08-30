using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreaseSpeed", menuName = "ScriptableObject/SkillEffect/IncreaseSpeed")]
    public class IncreaseSpeed : SkillEffect {
        private float SpeedUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase speed by {SpeedUpMultiplier * 100}% for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (BuffDuration < 0) {
                target.curSpeed += (int)(SpeedUpMultiplier * target.curSpeed);
            } else {
                target.AddEffect(new SpeedUp(source, target, SpeedUpMultiplier, BuffDuration));
            }
        }
    }
}