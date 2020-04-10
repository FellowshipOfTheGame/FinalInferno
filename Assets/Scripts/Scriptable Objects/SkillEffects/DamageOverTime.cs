using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DamageOverTime", menuName = "ScriptableObject/SkillEffect/DamageOverTime")]
    public class DamageOverTime : SkillEffect {
        // value1 = dmg multiplier
        // value2 = DoT duration
        public override string Description { get {return "Deals " + value1 + "x Neutral damage for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Neutral, (int)value2));
        }
    }
}