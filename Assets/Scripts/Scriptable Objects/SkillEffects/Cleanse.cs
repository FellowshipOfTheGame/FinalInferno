using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Cleanse", menuName = "ScriptableObject/SkillEffect/Cleanse")]
    public class Cleanse : SkillEffect {
        private bool ShouldRemoveDebuffs => value1 != 0;
        private bool ShouldRemoveUndesirable => value2 != 0;
        public override string Description {
            get {
                string desc = "Remove target's ";
                desc += ShouldRemoveDebuffs ? "debuffs" : "";
                desc += (ShouldRemoveDebuffs && ShouldRemoveUndesirable) ? " and " : "";
                desc += ShouldRemoveUndesirable ? "negative status effects" : "";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach (StatusEffect effect in target.effects.ToArray()) {
                if (!ShouldRemoveEffect(effect))
                    continue;
                effect.ForceRemove();
            }
        }

        private bool ShouldRemoveEffect(StatusEffect effect) {
            return (ShouldRemoveDebuffs && effect.Type == StatusType.Debuff) || (ShouldRemoveUndesirable && effect.Type == StatusType.Undesirable);
        }
    }
}
