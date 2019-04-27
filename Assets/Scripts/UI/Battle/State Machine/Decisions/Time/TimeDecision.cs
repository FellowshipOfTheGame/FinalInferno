using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada no tempo em que o estado está ativo.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Time")]
    public class TimeDecision : Decision
    {
        /// <summary>
        /// O tempo máximo que o estado pode ficar ativo.
        /// </summary>
        public float stateTime;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// Verifica se o tempo do estado já ultrapassou o limite máximo.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            return controller.CheckIfCountDownElapsed(stateTime);
        }

    }

}
