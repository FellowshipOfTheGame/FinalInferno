using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreasePowerExpo", menuName = "ScriptableObject/SkillEffect/DecreasePowerExponential")]
    public class DecreasePowerExpo : SkillEffect {
        // value1 = dmgDown multiplier
        // value2 = debuff duration
        public override string Description { get { return "Decrease power by " + value1*100 + "% every turn for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamageDownExponential(source, target, value1, (int)value2));
        }
    }
}