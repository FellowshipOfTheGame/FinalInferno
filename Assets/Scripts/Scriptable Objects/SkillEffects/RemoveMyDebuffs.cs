using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "RemoveMyDebuffs", menuName = "ScriptableObject/SkillEffect/RemoveMyDebuffs")]
    public class RemoveMyDebuffs : SkillEffect {
        // value1 = not used, but description is useful
        public override string Description => "Remove my debuffs";

        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach (StatusEffect statusEffect in target.effects.ToArray()) {
                if (statusEffect.Type == StatusType.Debuff && statusEffect.Source == source) {
                    statusEffect.Remove();
                }
            }
        }
    }
}