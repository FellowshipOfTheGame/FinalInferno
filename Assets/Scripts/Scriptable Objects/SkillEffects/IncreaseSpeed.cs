using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreaseSpeed", menuName = "ScriptableObject/SkillEffect/IncreaseSpeed")]
    public class IncreaseSpeed : SkillEffect {
        // value1 = speedUp multiplier
        // value2 = buff duration
        public override string Description { get { return "Increase speed by " + value1 * 100 + "% for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curSpeed += (int)value1 * target.curSpeed;
            else
                target.AddEffect(new SpeedUp(source, target, value1, (int)value2));
        }
    }
}