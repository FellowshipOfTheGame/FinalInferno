using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// A component that provides the required component of a ComponentRequester.
    /// </summary>
    public class ComponentProvider : MonoBehaviour
    {
        /// <summary>
        /// A reference to a component that requests another one.
        /// </summary>
        [SerializeField] private List<ComponentRequester> requesters;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            foreach (ComponentRequester requester in requesters)
                requester.RequestComponent(gameObject);
        }
    }

}
