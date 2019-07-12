using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Paralyze", menuName = "ScriptableObject/SkillEffect/Paralyze")]
    public class Paralyze : SkillEffect {
        // value1 = chance to parayze
        public override string Description1{ get {return " Chance";} }
        // value2 = duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Paralyzed(source, target, value1, (int)value2));
        }
    }
}