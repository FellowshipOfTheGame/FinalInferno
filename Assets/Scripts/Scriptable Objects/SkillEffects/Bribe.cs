using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Bribe", menuName = "ScriptableObject/SkillEffect/Bribe")]
    public class Bribe : SkillEffect {
        // value1 = chance to bribe
        // value2 = duration
        public override string Description { get { return "Bribe with " + value1*100 + "% of chance for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Bribed(source, target, value1, (int)value2));
        }
    }
}