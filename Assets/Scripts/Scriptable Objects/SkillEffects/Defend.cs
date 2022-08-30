using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Defend", menuName = "ScriptableObject/SkillEffect/Defend")]
    public class Defend : SkillEffect {
        private float DefUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase defense and resistance by {DefUpMultiplier * 100}% for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Defending(target, DefUpMultiplier, BuffDuration));
        }
    }
}