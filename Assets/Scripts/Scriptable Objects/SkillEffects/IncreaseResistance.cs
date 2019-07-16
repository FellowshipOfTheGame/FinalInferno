using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreaseResistance", menuName = "ScriptableObject/SkillEffect/IncreaseResistance")]
    public class IncreaseResistance : SkillEffect {
        // value1 = defUp multiplier
        public override string Description1{ get {return "x";} }
        // value2 = buff duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.curMagicDef += (int)value1 * target.curMagicDef;
            else
                target.AddEffect(new ResistanceUp(source, target, value1, (int)value2));
        }
    }
}