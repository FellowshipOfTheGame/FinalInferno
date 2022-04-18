using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Purge", menuName = "ScriptableObject/SkillEffect/Purge")]
    public class Purge : SkillEffect {
        private bool ShouldRemoveBuffs => value1 != 0;
        private bool ShouldRemoveNoTypeStatuses => value2 != 0;
        public override string Description {
            get {
                string desc = "Remove target's ";
                desc += ShouldRemoveBuffs ? "buffs" : "";
                desc += (ShouldRemoveBuffs && ShouldRemoveNoTypeStatuses) ? " and " : "";
                desc += ShouldRemoveNoTypeStatuses ? "neutral status effects" : "";
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
            return (ShouldRemoveBuffs && effect.Type == StatusType.Buff) || (ShouldRemoveNoTypeStatuses && effect.Type == StatusType.None);
        }
    }
}