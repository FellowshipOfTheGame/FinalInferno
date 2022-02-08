using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Revive", menuName = "ScriptableObject/SkillEffect/Revive")]
    public class Revive : SkillEffect {
        public override string Description => "Revive unit with 1 HP";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.Revive();
        }
    }
}
