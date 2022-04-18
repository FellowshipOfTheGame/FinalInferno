using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "HealOverTime", menuName = "ScriptableObject/SkillEffect/HealOverTime")]
    public class HealOverTime : SkillEffect {
        private float DmgMultiplier => value1;
        private int HoTDuration => (int)value2;
        public override string Description => $"Heals {DmgMultiplier}x damage for {HoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new HealingOverTime(source, target, DmgMultiplier, HoTDuration));
        }
    }
}