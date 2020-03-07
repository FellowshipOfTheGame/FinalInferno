using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Defend", menuName = "ScriptableObject/SkillEffect/Defend")]
    public class Defend : SkillEffect {
        // value1 = defUp multiplier
        // value2 = buff duration
        public override string Description { get { return "Increase defense and resistance by " + value1*100 + "% for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Defending(target, value1, (int)value2));
        }
    }
}