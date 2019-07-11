using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreaseDefense", menuName = "ScriptableObject/SkillEffect/DecreaseDefense")]
    public class DecreaseDefense : SkillEffect {
        // value1 = defDown multiplier
        public override string Description1{ get {return "x";} }
        // value2 = debuff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DefenseDown(source, target, value1, (int)value2));
        }
    }
}