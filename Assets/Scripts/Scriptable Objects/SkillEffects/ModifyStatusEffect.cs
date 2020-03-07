using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "ModifyStatusEffect", menuName = "ScriptableObject/SkillEffect/ModifyStatusEffect")]
    public class ModifyStatusEffect : SkillEffect {
        // value1 = index of status effect being changed
        // value2 = modifier to apply to the effect
        public override string Description { get { return "Modify effect selected by " + value2 + "x modifier"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.effects[Mathf.Clamp(((int)value1), 0, target.effects.Count - 1)].Amplify(value2);
        }
    }
}
