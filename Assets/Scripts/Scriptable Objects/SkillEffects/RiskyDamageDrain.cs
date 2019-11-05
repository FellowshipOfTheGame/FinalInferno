using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "RiskyDamageDrain", menuName = "ScriptableObject/SkillEffect/RiskyDamageDrain")]
    public class RiskyDamageDrain : SkillEffect {
        public override string Description0 { get { return "Drain "; } }
        // value1 = dmgDrain multiplier
        public override string Description1{ get {return "x";} }
        // value2 = debuff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            bool isDraining = false;
            foreach(StatusEffect effect in target.effects){
                if(effect.GetType() == typeof(DamageDrained) && effect.Source == source){
                    isDraining = true;
                    break;
                }
            }
            if(!isDraining){
                target.AddEffect(new DamageDrained(source, target, value1, (int)value2));
                source.AddEffect(new DrainingDamage(source, target, value1, (int)value2, true, true));
            }else{
            }
        }
    }
}