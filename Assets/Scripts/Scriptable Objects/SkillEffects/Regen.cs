using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Regen", menuName = "ScriptableObject/SkillEffect/Regen")]
    public class Regen : SkillEffect {
        // value1 = MaxHP multiplier
        // value2 = HoT duration
        public override string Description { get {return "Heals " + (value1 * 100) + "% of Target's Max HP every turn for " + value2 + " turns"; } }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Regenerating(source, target, value1, (int)value2));
        }
    }
}