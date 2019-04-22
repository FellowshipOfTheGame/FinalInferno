using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
	/// A component implementing a Mealy Finite State Machine.
	/// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// The current state of the machine.
        /// </summary>
        [SerializeField] private State currentState;

        /// <summary>
        /// The time elapsed since the beginning of the current state.
        /// </summary>
        private float stateTimeElapsed;

        /// <summary>
        /// Actions executed in the beginning of the execution.
        /// </summary>
        [SerializeField] private Action[] startActions;

        void Start()
        {
            // Executing all the start actions.
            foreach (Action A in startActions)
            {
                A.Act(this);
            }
        }

        void Update()
        {
            // Update the state time elapsed and verify for transitions
            stateTimeElapsed += Time.deltaTime;
            currentState.UpdateState(this);
        }

        /// <summary>
        /// Change the current state for another one and execute the 
        /// transition actions.
        /// </summary>
        /// <param name="nextState"> The new state of the machine. </param>
        /// <param name="transitionActions"> Actions executed in the transition. </param>
        public void TransitionToState(State nextState, Action[] transitionActions)
        {
            OnExitState(transitionActions);
            currentState = nextState;
            Debug.Log("New State: " + nextState.name);
        }

        /// <summary>
        /// Return if the state duration elapsed the duration.
        /// </summary>
        /// <param name="duration"> The duration to be verified. </param>
        public bool CheckIfCountDownElapsed(float duration)
        {
            return (stateTimeElapsed >= duration);
        }

        /// <summary>
        /// Reset the state countdown and execute the transition
        /// actions.
        /// </summary>
        /// <param name="transitionActions"> Actions executed in the transition. </param>
        private void OnExitState(Action[] transitionActions)
        {
            stateTimeElapsed = 0;
            foreach (Action A in transitionActions)
            {
                A.Act(this);
            }
        }
    }

}
