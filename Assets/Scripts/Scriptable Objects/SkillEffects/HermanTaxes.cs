using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "HermanTaxes", menuName = "ScriptableObject/SkillEffect/HermanTaxes")]
    public class HermanTaxes : SkillEffect {
        // value1 = strength of the buffs copied
        public override string Description { get { return "Herman receives " + value1*100 + "% of buffs received by allies"; } }
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach(BattleUnit unit in allies){
                if(unit != target)
                    unit.OnReceiveBuff += PayHerman;
            }
        }

        public void PayHerman(BattleUnit target, List<BattleUnit> units, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            if(units.Count < 1) return;

            BattleUnit herman = null;
            foreach(BattleUnit unit in BattleManager.instance.GetTeam(target)){
                if(unit.unit.name == "Herman"){
                    herman = unit;
                    break;
                }
            }
            if(herman != null){
                target.effects[Mathf.Clamp(((int)value1), 0, target.effects.Count - 1)].CopyTo(herman, value2);
            }
        }
    }
}
