using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/End Battle Callbacks")]
    public class EndBattleCallback : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// Indica que deve esperar a animação de skill acabar.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            foreach(BattleUnit battleUnit in BattleManager.instance.battleUnits){
                battleUnit.ResetMaxHP();
            }
            foreach(BattleUnit battleUnit in BattleManager.instance.queue.list){
                if(battleUnit.OnEndBattle != null)
                    battleUnit.OnEndBattle(battleUnit, BattleManager.instance.GetTeam(UnitType.Hero, true));
            }
        }

    }

}