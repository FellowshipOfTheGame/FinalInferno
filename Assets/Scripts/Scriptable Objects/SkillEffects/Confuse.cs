using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Confuse", menuName = "ScriptableObject/SkillEffect/Confuse")]
    public class Confuse : SkillEffect {
        private float ChanceToConfuse => value1;
        private int Duration => (int)value2;
        public override string Description => $"Confuse with {ChanceToConfuse * 100}% of chance for {Duration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Confused(source, target, ChanceToConfuse, Duration));
        }
    }
}