using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Paralyze", menuName = "ScriptableObject/SkillEffect/Paralyze")]
    public class Paralyze : SkillEffect {
        // value1 = chance to paralyze
        // value2 = duration
        public override string Description { get { return "Paralyze with " + value1*100 + "% of chance for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Paralyzed(source, target, value1, (int)value2));
        }
    }
}