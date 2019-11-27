using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "InitiativeBonus", menuName = "ScriptableObject/SkillEffect/InitiativeBonus")]
    public class InitiativeBonus : SkillEffect {
        // value1 = absolute points decrease
        // value2 = percentage speed to decrease points
        public override string Description { get { return "Decrease " + value1 + " + " + value2*100 + "% speed points"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            target.actionPoints -= Mathf.FloorToInt(value1);
            target.actionPoints -= Mathf.FloorToInt(value2 * target.curSpeed);
            BattleManager.instance.queue.Sort();
        }
    }
}