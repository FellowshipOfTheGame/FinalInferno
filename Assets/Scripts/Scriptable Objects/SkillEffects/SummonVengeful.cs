using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "SummonVengeful", menuName = "ScriptableObject/SkillEffect/Summon Vengeful")]
    public class SummonVengeful : SkillEffect {
        // value1 = dmg modifier of ghost
        // value2 = status duration
        public override string Description {
            get {
                string val = "Summons target as a vengeful ghost with " + value1 + "x damage";
                val += (value2 < 0)? " until the end of the battle or" : (" for " + value2 + " turns or until");
                val += " unit is revived";
                return val;
            }
        }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new VengefulGhost(source, target, value1, (int)value2));
        }
    }
}