﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que recoloca a unidade atual na fila como se ela tivesse atacado.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Give Action Points")]
    public class GiveActionPoints : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        [SerializeField] private bool forceAttack = false;
        public override void Act(StateController controller)
        {
            BattleUnit currentUnit = BattleManager.instance.currentUnit;
            // Quando a unidade morre por usar uma skill ou status effect currentUnit==null
            if(currentUnit == null){
                currentUnit = BattleSkillManager.currentUser;
                // Quando a unidade morre por status effect currentUnit==null aqui e nada deve ser feito
                if(currentUnit != null){
                    Debug.Log("Unidade morreu por conta de counter ou algo do tipo");
                    currentUnit.actionPoints += Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - currentUnit.ActionCostReduction));
                    BattleManager.instance.UpdateQueue(0, true);
                }
            }else{
                if(forceAttack){
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(currentUnit.unit.attackSkill.cost * (1.0f - currentUnit.ActionCostReduction) ));
                }else{
                    BattleManager.instance.UpdateQueue(Mathf.FloorToInt(BattleSkillManager.currentSkill.cost * (1.0f - currentUnit.ActionCostReduction) ));
                }
            }

            foreach(BattleUnit battleUnit in BattleSkillManager.currentTargets){
                battleUnit.GetComponent<AxisInteractableItem>().DisableReference();
            }
        }

    }

}
