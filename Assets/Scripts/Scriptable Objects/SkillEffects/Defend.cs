using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Defend", menuName = "ScriptableObject/SkillEffect/Defend")]
    public class Defend : SkillEffect {
        public override string Description0 { get { return "Increase defense by "; } }
        // value1 = defUp multiplier
        public override string Description1{ get {return "%";} }
        // value2 = buff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Defending(target, value1, (int)value2));
        }
    }
}