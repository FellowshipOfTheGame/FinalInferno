using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "RiskyDamageDrain", menuName = "ScriptableObject/SkillEffect/RiskyDamageDrain")]
    public class RiskyDamageDrain : SkillEffect {
        // value1 = dmgDrain multiplier
        // value2 = debuff duration
        public override string Description { get { return "Drain " + value1*100 + "% damage for " + value2 + " turns, but gets drained instead if target is alive at the end"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            bool isDraining = false;
            foreach(StatusEffect effect in target.effects){
                if(effect.GetType() == typeof(DamageDrained) && effect.Source == source){
                    isDraining = true;
                    break;
                }
            }
            if(!isDraining){
                source.AddEffect(new DrainingDamage(source, target, value1, (int)value2, false, true, true));
                target.AddEffect(new DamageDrained(source, target, value1, (int)value2));
            }
        }
    }
}