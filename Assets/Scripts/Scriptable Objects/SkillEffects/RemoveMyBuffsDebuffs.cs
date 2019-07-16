using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "RemoveMyBuffsDebuffs", menuName = "ScriptableObject/SkillEffect/RemoveMyBuffsDebuffs")]
    public class RemoveMyBuffsDebuffs : SkillEffect {
        // value1 = not used, but description is useful
        public override string Description1{ get {return "\bRemove my buffs and debuffs";} }
        public override string Description2{ get{ return null; } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach(StatusEffect statusEffect in target.effects){
                if((statusEffect.Type == StatusType.Buff || statusEffect.Type == StatusType.Debuff) && statusEffect.Source == source)
                    statusEffect.Remove();
            }
        }
    }
}