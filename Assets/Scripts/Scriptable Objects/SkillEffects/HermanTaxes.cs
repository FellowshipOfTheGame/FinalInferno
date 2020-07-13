using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "HermanTaxes", menuName = "ScriptableObject/SkillEffect/HermanTaxes")]
    public class HermanTaxes : SkillEffect {
        // value1 = strength of the buffs copied
        public override string Description { get { return "Herman receives " + value1*100 + "% of buffs and heals received by allies"; } }
        [SerializeField] GameObject visualEffect = null;
        
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach(BattleUnit unit in allies){
                if(unit != target){
                    unit.OnReceiveBuff += PayHerman;
                    unit.OnHeal += FeedHerman;
                }
            }
        }

        public void FeedHerman(BattleUnit target, List<BattleUnit> units, bool shouldOverride1 = false, float val1 = 0f, bool shouldOverride2 = false, float val2 = 0f){
            if(units.Count < 1) return;

            BattleUnit herman = BattleManager.instance.GetTeam(target).Find(unit => unit.Unit.name == "Herman");
            if(herman != null){
                herman.Heal((int)val1, value1);
                if(visualEffect){
                    GameObject obj = GameObject.Instantiate(visualEffect, herman.transform.parent);
                    obj.GetComponent<SkillVFX>().forceCallback = true;
                    obj.GetComponent<SpriteRenderer>().sortingOrder = herman.GetComponent<SpriteRenderer>().sortingOrder + 2;
                }
            }
        }

        public void PayHerman(BattleUnit target, List<BattleUnit> units, bool shouldOverride1 = false, float val1 = 0f, bool shouldOverride2 = false, float val2 = 0f){
            if(units.Count < 1) return;

            BattleUnit herman = BattleManager.instance.GetTeam(target).Find(unit => unit.Unit.name == "Herman");
            if(herman != null){
                target.effects[Mathf.Clamp(((int)val1), 0, target.effects.Count - 1)].CopyTo(herman, value1);
                if(visualEffect){
                    GameObject obj = GameObject.Instantiate(visualEffect, herman.transform.parent);
                    obj.GetComponent<SkillVFX>().forceCallback = true;
                    obj.GetComponent<SpriteRenderer>().sortingOrder = herman.GetComponent<SpriteRenderer>().sortingOrder + 2;
                }
            }
        }
    }
}
