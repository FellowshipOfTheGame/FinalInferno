using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Componente que provê outro ao requisitador.
    /// </summary>
    public class ComponentProvider : MonoBehaviour
    {
        /// <summary>
        /// Referência aos componentes que requerem algum componente.
        /// </summary>
        [SerializeField] private List<ComponentRequester> requesters;

        void Awake()
        {
            foreach (ComponentRequester requester in requesters)
                requester.RequestComponent(gameObject);
        }
    }

}
