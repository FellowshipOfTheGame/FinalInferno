using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreaseStatusResistance", menuName = "ScriptableObject/SkillEffect/IncreaseStatusResistance")]
    public class IncreaseStatusResistance : SkillEffect {
        // value1 = additive status resistance increase
        public override string Description1{ get {return "x";} }
        // value2 = buff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.statusResistance += value1;
            else
                target.AddEffect(new DefenseUp(source, target, value1, (int)value2));
        }
    }
}