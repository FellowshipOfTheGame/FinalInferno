using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObject/SkillEffect/Damage")]
    public class Damage : SkillEffect {
        // value1 = dmg multiplier
        public override string Description1{ get {return "x";} }
        public override string Description2{ get {return null;} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.TakeDamage(source.curDmg, value1, DamageType.Physical, Element.Neutral);
        }
    }
}
