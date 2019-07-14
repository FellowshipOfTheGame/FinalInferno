using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "ModifyStatusEffect", menuName = "ScriptableObject/SkillEffect/ModifyStatusEffect")]
    public class ModifyStatusEffect : SkillEffect {
        // value1 = index of status effect being changed
        public override string Description1{ get {return "\bEffect number " + value1;} }
        // value1 = modifier to apply to the effect
        public override string Description2{ get {return "x";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.effects[Mathf.Clamp(((int)value1), 0, target.effects.Count - 1)].Amplify(value2);
        }
    }
}
