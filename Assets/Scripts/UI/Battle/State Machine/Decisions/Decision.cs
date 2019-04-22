using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// A component abstracting the structure of a decision object.
    /// </summary>
    public abstract class Decision : ScriptableObject
    {
        /// <summary>
        /// Verify if the decision triggers.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public abstract bool Decide(StateController controller);
    }

}
