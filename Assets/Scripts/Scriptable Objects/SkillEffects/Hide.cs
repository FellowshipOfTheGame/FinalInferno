using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Hide", menuName = "ScriptableObject/SkillEffect/Hide")]
    public class Hide : SkillEffect {
        // value1 = Negative aggro each turn
        public override string Description1{ get {return " aggro";} }
        // value2 = status duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Hiding(source, target, value1, (int)value2));
        }
    }
}