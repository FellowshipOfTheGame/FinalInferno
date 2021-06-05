using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/SkillEffect/Heal")]
    public class Heal : SkillEffect {
        // value1 = dmg multiplier for heal
        // value2 = health percentage heal
        public override string Description {
            get {
                string desc = "Increase current HP by ";
                desc += (value1 != 0)? (value1*100 + "% of user's power") : "";
                desc += (value1 != 0 && value2 != 0)? " plus " : "";
                desc += (value2 != 0)? value2*100 + "% of target's max HP" : "";
                return  desc;
            }
        }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value1 != 0)
                target.Heal(source.curDmg, value1, source);
            if(value2 != 0)
                target.Heal(target.Unit.hpMax, value2, source);
        }
    }
}
