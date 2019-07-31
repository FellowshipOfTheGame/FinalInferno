using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreasePower", menuName = "ScriptableObject/SkillEffect/IncreasePower")]
    public class IncreasePower : SkillEffect {
        // value1 = dmgUp multiplier
        public override string Description1{ get {return "x";} }
        // value2 = buff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curDmg += (int)value1 * target.curDmg;
            else
                target.AddEffect(new DamageUp(source, target, value1, (int)value2));
        }
    }
}