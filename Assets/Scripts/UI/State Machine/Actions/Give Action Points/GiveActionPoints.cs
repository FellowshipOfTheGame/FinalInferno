using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Give Action Points")]
    public class GiveActionPoints : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            BattleUnit currentUnit = BattleManager.instance.currentUnit;
            BattleManager.instance.UpdateQueue(Mathf.FloorToInt(currentUnit.unit.attackSkill.cost * (1.0f - currentUnit.ActionCostReduction) ));
        }

    }

}
