using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que recoloca a unidade atual na fila como se ela tivesse atacado.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Select Next Unit")]
    public class SelectNextUnit : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller)
        {
            BattleManager.instance.UpdateQueue(0, true);
        }

    }

}