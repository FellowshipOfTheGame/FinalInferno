using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.EventSystem {
    [CreateAssetMenu(fileName = "New Event", menuName = "EventSystem/Event")]
    public class EventFI : ScriptableObject {
        private readonly List<IEventListenerFI> listeners = new List<IEventListenerFI>();

        public void Raise() {
            foreach (IEventListenerFI listener in listeners.ToArray()) {
                listener.OnEventRaised();
            }
        }

        public void AddListener(IEventListenerFI newListener) {
            if (!listeners.Contains(newListener)) {
                listeners.Add(newListener);
            }
        }

        public void RemoveListener(IEventListenerFI listenerToRemove) {
            if (listeners.Contains(listenerToRemove)) {
                listeners.Remove(listenerToRemove);
            }
        }
    }
}