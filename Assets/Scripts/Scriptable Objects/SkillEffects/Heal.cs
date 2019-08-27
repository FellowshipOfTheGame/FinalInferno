using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/SkillEffect/Heal")]
    public class Heal : SkillEffect {
        public override string Description0 { get { return "Increase life by "; } }
        // value1 = dmg multiplier for heal
        public override string Description1{ get {return "%";} }
        // value2 = health percentage heal
        public override string Description2{ get {return "% maxHP";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.TakeDamage(source.curDmg, -value1, DamageType.None, Element.Neutral, source);
            target.TakeDamage(target.unit.hpMax, -value2, DamageType.None, Element.Neutral, source);
        }
    }
}
