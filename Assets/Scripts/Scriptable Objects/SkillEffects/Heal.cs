using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/SkillEffect/Heal")]
    public class Heal : SkillEffect {
        // value1 = dmg multiplier for heal
        // value2 = health percentage heal
        public override string Description { get { return "Increase life by " + value1*100 + "% of user's power plus " + value2*100 + "% of target's max HP"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.TakeDamage(source.curDmg, -value1, DamageType.None, Element.Neutral, source);
            target.TakeDamage(target.unit.hpMax, -value2, DamageType.None, Element.Neutral, source);
        }
    }
}
