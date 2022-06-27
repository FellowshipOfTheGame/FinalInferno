using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem {
    [System.Serializable]
    public class VoidEventListenerFI : IEventListenerFI {
        [SerializeField] private EventFI eventListened = null;
        [SerializeField] private UnityEvent eventCallback = new UnityEvent();

        public VoidEventListenerFI(EventFI eventToListen, UnityAction response) {
            eventListened = eventToListen;
            eventCallback = new UnityEvent();
            eventCallback.AddListener(response);
        }

        public void OnEventRaised() {
            eventCallback?.Invoke();
        }

        public void StartListeningEvent() {
            if (eventListened)
                eventListened.AddListener(this);
        }

        public void StopListeningEvent() {
            if (eventListened)
                eventListened.RemoveListener(this);
        }
    }
}
