using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Purge", menuName = "ScriptableObject/SkillEffect/Purge")]
    public class Purge : SkillEffect {
        // value1 = should remove buffs: 0 = no; any other value = yes;
        // value2 = should remove no type statuses: 0 = no; any other value = yes;
        public override string Description {
            get {
                bool removeBuffs = value1 != 0;
                bool removeNone = value2 != 0;
                string desc = "Remove target's ";
                desc += (removeBuffs) ? "buffs" : "";
                desc += (removeBuffs && removeNone) ? " and " : "";
                desc += (removeNone) ? "neutral status effects" : "";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            bool removeBuffs = value1 != 0;
            bool removeNone = value2 != 0;
            foreach (StatusEffect effect in target.effects.ToArray()) {
                if ((removeBuffs && effect.Type == StatusType.Debuff) || (removeNone && effect.Type == StatusType.None)) {
                    effect.ForceRemove();
                }
            }
        }
    }
}