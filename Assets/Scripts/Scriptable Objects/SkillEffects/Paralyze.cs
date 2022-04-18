using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Paralyze", menuName = "ScriptableObject/SkillEffect/Paralyze")]
    public class Paralyze : SkillEffect {
        private float ChanceToParalyze => value1;
        private int Duration => (int)value2;
        public override string Description => $"Paralyze with {ChanceToParalyze * 100}% of chance for {Duration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Paralyzed(source, target, ChanceToParalyze, Duration));
        }
    }
}