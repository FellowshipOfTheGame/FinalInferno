using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Hide", menuName = "ScriptableObject/SkillEffect/Hide")]
    public class Hide : SkillEffect {
        // value1 = Negative aggro each turn
        // value2 = status duration
        public override string Description { get { return "Decrease " + value1 + " aggro for " + value2 + " turns"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Hiding(source, target, value1, (int)value2));
        }
    }
}