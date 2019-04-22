using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// A component abstracting the structure of a action object.
    /// </summary>
    public abstract class Action : ScriptableObject 
    {
        /// <summary>
        /// Execute an action.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public abstract void Act (StateController controller);
    }

}
