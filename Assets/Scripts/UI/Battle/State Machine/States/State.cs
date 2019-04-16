using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// A component implementing a state of the state machine.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/State")]
    public class State : ScriptableObject
    {
        /// <summary>
        /// All the transitions corresponding to the state.
        /// </summary>
        public Transition[] transitions;

        /// <summary>
        /// Funcion called all the frames the state is activated.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public void UpdateState(StateController controller)
        {
            CheckTransitions(controller);
        }

        /// <summary>
        /// Verify the decision triggers and execute the transition, if needed.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        private void CheckTransitions(StateController controller)
        {
            foreach (Transition T in transitions)
            {
                bool decisionSucceeded = true;
                foreach (Decision D in T.decisions)
                {
                    decisionSucceeded = decisionSucceeded && D.Decide(controller);
                }

                if (decisionSucceeded)
                {
                    controller.TransitionToState(T.nextState, T.actions);
                }
            }
        }

    }

}
