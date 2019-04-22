using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// An action that require a type of component.
    /// </summary>
    public abstract class ComponentRequester : Action
    {
        /// <summary>
        /// Funcion called to request a component to the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        public abstract void RequestComponent(GameObject provider);
    }

}