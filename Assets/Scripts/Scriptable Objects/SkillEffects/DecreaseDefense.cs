using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreaseDefense", menuName = "ScriptableObject/SkillEffect/DecreaseDefense")]
    public class DecreaseDefense : SkillEffect {
        // value1 = defDown multiplier
        // value2 = debuff duration
        public override string Description { get { return "Decrease defense by " + value1*100 + "% for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curDef -= (int)value1 * target.curDef;
            else
                target.AddEffect(new DefenseDown(source, target, value1, (int)value2));
        }
    }
}