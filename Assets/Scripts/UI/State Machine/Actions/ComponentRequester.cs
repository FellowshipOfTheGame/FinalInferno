using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Tipo de ação que requer algum componente.
    /// </summary>
    public abstract class ComponentRequester : Action
    {
        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public abstract void RequestComponent(GameObject provider);
    }

}