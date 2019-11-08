using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreasePower", menuName = "ScriptableObject/SkillEffect/DecreasePower")]
    public class DecreasePower : SkillEffect {
        // value1 = dmgDown multiplier
        // value2 = debuff duration
        public override string Description { get { return "Decrease power by " + value1*100 + "% for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curDmg -= (int)value1 * target.curDmg;
            else
                target.AddEffect(new DamageDown(source, target, value1, (int)value2));
        }
    }
}