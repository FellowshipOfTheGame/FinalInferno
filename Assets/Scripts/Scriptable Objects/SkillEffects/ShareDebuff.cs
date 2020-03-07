using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "ShareDebuff", menuName = "ScriptableObject/SkillEffect/ShareDebuff")]
    public class ShareDebuff : SkillEffect {
        // value1 = index of status effect being shared
        // value2 = modifier to apply to the effect
        public override string Description { get { return "Share " + value2*100 + "% of selected status effect to allies"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach(BattleUnit unit in allies){
                if(unit != target)
                    target.effects[Mathf.Clamp(((int)value1), 0, target.effects.Count - 1)].CopyTo(unit, value2);
            }
        }
    }
}
