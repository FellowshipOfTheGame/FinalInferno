using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "InitiativeBonus", menuName = "ScriptableObject/SkillEffect/InitiativeBonus")]
    public class InitiativeBonus : SkillEffect {
        // value1 = absolute points decrease
        // value2 = percentage speed to decrease points
        public override string Description {
            get {
                string desc = "Decrease action points by ";
                desc += (value1 != 0)? (value1 + "") : "";
                desc += (value1 != 0 && value2 != 0)? "+ " : "";
                desc += (value2 != 0)? (value2*100 + "% speed") : "";
                return desc;
            }
        }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.actionPoints -= Mathf.FloorToInt(value1);
            target.actionPoints -= Mathf.FloorToInt(value2 * target.curSpeed);
            BattleManager.instance.queue.Sort();
        }
    }
}