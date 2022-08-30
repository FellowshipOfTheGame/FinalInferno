using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Bribe", menuName = "ScriptableObject/SkillEffect/Bribe")]
    public class Bribe : SkillEffect {
        private float ChanceToBribe => value1;
        private int Duration => (int)value2;
        public override string Description => $"Bribe with {ChanceToBribe * 100}% of chance for {Duration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Bribed(source, target, ChanceToBribe, Duration));
        }
    }
}