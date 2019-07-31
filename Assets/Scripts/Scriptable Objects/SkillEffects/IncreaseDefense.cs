using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreaseDefense", menuName = "ScriptableObject/SkillEffect/IncreaseDefense")]
    public class IncreaseDefense : SkillEffect {
        // value1 = defUp multiplier
        public override string Description1{ get {return "%";} }
        // value2 = buff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curDef += (int)value1 * target.curDef;
            else
                target.AddEffect(new DefenseUp(source, target, value1, (int)value2));
        }
    }
}