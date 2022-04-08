using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem {
    public class EventListenerFI : MonoBehaviour, IEventListenerFI {
        [SerializeField] private EventFI _event;
        [SerializeField] private UnityEvent _response;

        private void OnEnable() {
            _event?.AddListener(this);
        }

        private void OnDisable() {
            _event?.RemoveListener(this);
        }

        public void OnEventRaised() {
            _response?.Invoke();
        }
    }
}