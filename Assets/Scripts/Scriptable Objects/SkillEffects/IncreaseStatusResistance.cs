using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "IncreaseStatusResistance", menuName = "ScriptableObject/SkillEffect/IncreaseStatusResistance")]
    public class IncreaseStatusResistance : SkillEffect {
        // value1 = additive status resistance increase
        // value2 = buff duration
        public override string Description {
            get{
                string desc = "Increase resistance to abnormal effects by " + value1 * 100 + "% for ";
                desc += (value2 > 0)? (value2 + " turns") : "rest of battle";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            if(value2 < 0)
                target.statusResistance += value1;
            else
                target.AddEffect(new StatusResistUp(source, target, value1, (int)value2));
        }
    }
}