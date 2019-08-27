using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreaseDefense", menuName = "ScriptableObject/SkillEffect/DecreaseDefense")]
    public class DecreaseDefense : SkillEffect {
        public override string Description0 { get { return "Decrease defense by "; } }
        // value1 = defDown multiplier
        public override string Description1{ get {return "%";} }
        // value2 = debuff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curDef -= (int)value1 * target.curDef;
            else
                target.AddEffect(new DefenseDown(source, target, value1, (int)value2));
        }
    }
}