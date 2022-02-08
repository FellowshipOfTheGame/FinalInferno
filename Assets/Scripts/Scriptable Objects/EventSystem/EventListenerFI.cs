using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem {
    public class EventListenerFI : MonoBehaviour, IEventListenerFI {
        [SerializeField] private EventFI _event;
        [SerializeField] private UnityEvent _response;

        // Start is called before the first frame update

        private void OnEnable() {
            if (_event != null) {
                _event.AddListener(this);
            }
        }

        private void OnDisable() {
            if (_event != null) {
                _event.RemoveListener(this);
            }
        }

        public void OnEventRaised() {
            if (_response != null) {
                _response.Invoke();
            }
        }
    }
}