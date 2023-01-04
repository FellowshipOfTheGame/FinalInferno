using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    public class ComponentProvider : MonoBehaviour {
        [SerializeField] private List<ComponentRequester> requesters;

        private void Awake() {
            UpdateComponent();
        }

        public void UpdateComponent() {
            foreach (ComponentRequester requester in requesters) {
                requester.RequestComponent(gameObject);
            }
        }
    }
}
