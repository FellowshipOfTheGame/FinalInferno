using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    public class ComponentProvider : MonoBehaviour
    {
        [SerializeField] private ComponentRequester requester;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            requester.RequestComponent(gameObject);
        }
    }

}
