﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "DamageOverTime", menuName = "ScriptableObject/SkillEffect/DamageOverTime")]
    public class DamageOverTime : SkillEffect {
        // value1 = dmg multiplier
        public override string Description1{ get {return "x Damage";} }
        // value2 = DoT duration
        public override string Description2{ get {return " turns";} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamagingOverTime(source, target, value1, Element.Neutral, (int)value2));
        }
    }
}