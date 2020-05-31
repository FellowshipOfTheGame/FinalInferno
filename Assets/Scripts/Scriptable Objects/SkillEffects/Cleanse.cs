using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Cleanse", menuName = "ScriptableObject/SkillEffect/Cleanse")]
    public class Cleanse : SkillEffect {
        // value1 = should remove debuffs: 0 = no; any other value = yes;
        // value2 = should remove undesirable statuses: 0 = no; any other value = yes;
        public override string Description {
            get{
                bool removeDebuffs = value1 != 0;
                bool removeUndesirable = value2 != 0;
                string desc = "Remove target's ";
                desc += (removeDebuffs)? "debuffs" : "";
                desc += (removeDebuffs && removeUndesirable)? " and " : "";
                desc += (removeUndesirable)? "negative status effects" : "";
                return desc;
            }
        }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            bool removeDebuffs = value1 != 0;
            bool removeUndesirable = value2 != 0;
            foreach(StatusEffect effect in target.effects.ToArray()){
                if((removeDebuffs && effect.Type == StatusType.Debuff) || (removeUndesirable && effect.Type == StatusType.Undesirable)){
                    effect.ForceRemove();
                }
            }
        }
    }
}
