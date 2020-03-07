using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Suffocation", menuName = "ScriptableObject/SkillEffect/Suffocation")]
    public class Suffocation : SkillEffect {
        // value1 = dmg multiplier
        // value2 = DoT duration
        public override string Description { get { return "Deals " + value1 + "x Wind damage for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Wind, (int)value2));
        }
    }
}