using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "RemoveMyDebuffs", menuName = "ScriptableObject/SkillEffect/RemoveMyDebuffs")]
    public class RemoveMyDebuffs : SkillEffect {
        public override string Description => "Remove debuffs applied by this unit";

        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach (StatusEffect statusEffect in target.effects.ToArray()) {
                if (statusEffect.Type == StatusType.Debuff && statusEffect.Source == source) {
                    statusEffect.Remove();
                }
            }
        }
    }
}