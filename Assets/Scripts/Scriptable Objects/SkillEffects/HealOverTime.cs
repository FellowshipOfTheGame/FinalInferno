using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "HealOverTime", menuName = "ScriptableObject/SkillEffect/HealOverTime")]
    public class HealOverTime : SkillEffect {
        // value1 = dmg multiplier
        // value2 = HoT duration
        public override string Description { get {return "Heals " + value1 + "x damage for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new HealingOverTime(source, target, value1, (int)value2));
        }
    }
}