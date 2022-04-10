using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem {
    public class EventListenerFI : MonoBehaviour, IEventListenerFI {
        [SerializeField] private EventFI _event;
        [SerializeField] private UnityEvent _response;

        private void OnEnable() {
            if (_event)
                _event.AddListener(this);
        }

        private void OnDisable() {
            if (_event)
                _event.RemoveListener(this);
        }

        public void OnEventRaised() {
            _response?.Invoke();
        }
    }
}