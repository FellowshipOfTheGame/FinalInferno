using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Hypothermia", menuName = "ScriptableObject/SkillEffect/Hypothermia")]
    public class Hypothermia : SkillEffect {
        // value1 = dmg multiplier
        // value2 = DoT duration
        public override string Description { get { return "Deals " + value1 + "x Water damage for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Water, (int)value2));
        }
    }
}