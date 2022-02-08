using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Componente que provê outro ao requisitador.
    /// </summary>
    public class ComponentProvider : MonoBehaviour {
        /// <summary>
        /// Referência aos componentes que requerem algum componente.
        /// </summary>
        [SerializeField] private List<ComponentRequester> requesters;

        private void Awake() {
            UpdateComponent();
        }

        public void UpdateComponent(GameObject target = null) {
            if (target == null) {
                target = gameObject;
            }

            foreach (ComponentRequester requester in requesters) {
                requester.RequestComponent(target);
            }
        }
    }

}
