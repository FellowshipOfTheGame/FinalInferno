using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// A component that store transition specifications.
    /// </summary>
    [System.Serializable]
    public class Transition
    {
        /// <summary>
        /// The decisions to trigger the transition.
        /// All decisions must be true to activate the trigger.
        /// </summary>
        public Decision[] decisions;

        /// <summary>
        /// The new state of the state machine.
        /// </summary>
        public State nextState;

        /// <summary>
        /// Actions that activate in the transition.
        /// </summary>
        public Action[] actions;
    }

}
