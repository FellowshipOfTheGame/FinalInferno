using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Eunuch Satyr", menuName = "ScriptableObject/Enemy/EunuchSatyr")]
    public class EunuchSatyr : Enemy
    {
        private bool IsDrainingSpeed(BattleUnit unit){
            if(unit == null || !(unit.unit is EunuchSatyr)) return false;
            foreach(StatusEffect effect in unit.effects){
                if(effect is DrainingSpeed && effect.Source == unit){
                    return true;
                }
            }
            return false;
        }

        public override Skill AttackDecision(){
            BattleUnit thisUnit = BattleManager.instance.currentUnit;
            return (IsDrainingSpeed(thisUnit))? attackSkill : skills[0];
        }
    }
}
