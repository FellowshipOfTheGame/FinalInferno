using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Revive", menuName = "ScriptableObject/SkillEffect/Revive")]
    public class Revive : SkillEffect {
        // value1 = not used, but description is useful
        public override string Description1{ get {return "\bRevive unit with 1 HP";} }
        public override string Description2{ get {return null;} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.Revive();
        }
    }
}
