using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decision based on the time the state is active.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Time")]
    public class TimeDecision : Decision
    {
        /// <summary>
        /// The maximum time the state will remain active.
        /// </summary>
        public float stateTime;

        /// <summary>
        /// Verify if the decision triggers.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override bool Decide(StateController controller)
        {
            return CheckStateTime(controller);
        }

        /// <summary>
        /// Verify if the current state time elapsed the maximum time of the state.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        private bool CheckStateTime(StateController controller)
        {
            return controller.CheckIfCountDownElapsed(stateTime);
        }
    }

}
