using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "RemoveMyDebuffs", menuName = "ScriptableObject/SkillEffect/RemoveMyDebuffs")]
    public class RemoveMyDebuffs : SkillEffect {
        public override string Description0 { get { return ""; } }
        // value1 = not used, but description is useful
        public override string Description1{ get {return "\bRemove my debuffs";} }
        public override string Description2{ get{ return null; } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            foreach(StatusEffect statusEffect in target.effects.ToArray()){
                if(statusEffect.Type == StatusType.Debuff && statusEffect.Source == source)
                    statusEffect.Remove();
            }
        }
    }
}