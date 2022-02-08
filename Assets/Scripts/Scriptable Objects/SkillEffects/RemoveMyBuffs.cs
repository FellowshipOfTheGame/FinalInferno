using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "RemoveMyBuffs", menuName = "ScriptableObject/SkillEffect/RemoveMyBuffs")]
    public class RemoveMyBuffs : SkillEffect {
        // value1 = not used, but description is useful
        public override string Description => "Remove my buffs";

        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach (StatusEffect statusEffect in target.effects.ToArray()) {
                if (statusEffect.Type == StatusType.Buff && statusEffect.Source == source) {
                    statusEffect.Remove();
                }
            }
        }
    }
}