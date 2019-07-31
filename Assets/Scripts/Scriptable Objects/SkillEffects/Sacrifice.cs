using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Sacrifice", menuName = "ScriptableObject/SkillEffect/Sacrifice")]
    public class Sacrifice : SkillEffect {
        // value1 = percentage of max HP sacrificed
        public override string Description1{ get {return "% maxHP";} }
        // value2 = who receives the heal: 0 = target's allies; 1 = target's enemies;
        public override string Description2{ get {return "\bHeal target's " +( (((int)value2 % 2) == 0)? "allies" : "enemies" ); } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            int damage = target.DecreaseHP(value1);
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            int healValue;
            switch((int)value2 % 2){
                case 0: // If the heal is to allies
                    healValue = (allies.Count > 1)? damage / (allies.Count - 1) : 0;
                    foreach(BattleUnit unit in allies){
                        if(unit != target)
                            unit.TakeDamage(healValue, -1.0f, DamageType.None, Element.Neutral, target);
                    }
                    break;
                case 1: // If the heal is to enemies
                    int nEnemies = (BattleManager.instance.queue.Count() - allies.Count);
                    healValue = (nEnemies > 0)? damage / nEnemies : 0;
                    foreach(BattleUnit unit in BattleManager.instance.queue.list){
                        if(!allies.Contains(unit))
                            unit.TakeDamage(healValue, -1.0f, DamageType.None, Element.Neutral, target);
                    }
                    break;
            }
        }
    }
}
