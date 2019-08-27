using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "HermanTaxes", menuName = "ScriptableObject/SkillEffect/HermanTaxes")]
    public class HermanTaxes : SkillEffect {
        public override string Description0 { get { return "Pay Herman by "; } }
        // value1 = strength of the buffs copied
        public override string Description1{ get {return "x";} }
        public override string Description2{ get {return null;} }
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach(BattleUnit unit in allies){
                if(unit != target)
                    unit.OnReceiveBuff += PayHerman;
            }
        }

        public void PayHerman(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0f, bool shouldOverride2 = false, float value2 = 0f){
            if(targets.Count < 1) return;

            BattleUnit target = targets[0];
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
