using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreaseSpeed", menuName = "ScriptableObject/SkillEffect/DecreaseSpeed")]
    public class DecreaseSpeed : SkillEffect {
        // value1 = speedDown multiplier
        // value2 = debuff duration
        public override string Description { get { return "Decrease speed by " + value1*100 + "% for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curSpeed -= (int)value1 * target.curSpeed;
            else
                target.AddEffect(new SpeedDown(source, target, value1, (int)value2));
        }
    }
}