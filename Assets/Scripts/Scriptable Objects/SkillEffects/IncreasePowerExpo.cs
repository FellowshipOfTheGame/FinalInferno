using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreasePowerExpo", menuName = "ScriptableObject/SkillEffect/IncreasePowerExponential")]
    public class IncreasePowerExpo : SkillEffect {
        // value1 = dmgUp multiplier
        // value2 = buff duration
        public override string Description { get { return "Increase power by " + value1 * 100 + "% every turn for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamageUpExponential(source, target, value1, (int)value2));
        }
    }
}