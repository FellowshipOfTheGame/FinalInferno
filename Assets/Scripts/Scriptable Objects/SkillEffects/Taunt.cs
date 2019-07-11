using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Taunt", menuName = "ScriptableObject/SkillEffect/Taunt")]
    public class Taunt : SkillEffect {
        // value1 = Aggro gain per turn
        public override string Description1{ get {return "%";} }
        // value2 = status duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Taunting(source, target, value1, (int)value2));
        }
    }
}