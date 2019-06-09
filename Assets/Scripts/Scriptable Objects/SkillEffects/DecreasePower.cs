using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DecreasePower", menuName = "ScriptableObject/SkillEffect/DecreasePower")]
    public class DecreasePower : SkillEffect {
        // value1 = dmgDown multiplier
        public override string Description1{ get {return "%";} }
        // value2 = debuff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamageDown(target, value1, (int)value2));
        }
    }
}