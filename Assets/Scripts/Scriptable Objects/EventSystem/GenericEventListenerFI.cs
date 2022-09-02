using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.EventSystem {
    [System.Serializable]
    public class GenericEventListenerFI<T> : IGenericEventListenerFI<T> {
        [SerializeField] private GenericEventFI<T> eventListened = null;
        [SerializeField] private UnityEvent<T> eventCallback = new UnityEvent<T>();

        public GenericEventListenerFI(GenericEventFI<T> eventToListen, UnityAction<T> response) {
            eventListened = eventToListen;
            eventCallback = new UnityEvent<T>();
            eventCallback.AddListener(response);
        }

        public void OnEventRaised(T value) {
            eventCallback?.Invoke(value);
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
