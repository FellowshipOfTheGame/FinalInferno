using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.EventSystem {
    public abstract class GenericEventFI<T> : ScriptableObject {
        [SerializeField] private T debugValue;
        private readonly List<IGenericEventListenerFI<T>> listeners = new List<IGenericEventListenerFI<T>>();

        public void Raise(T value) {
            foreach (IGenericEventListenerFI<T> listener in listeners.ToArray()) {
                if (listener != null)
                    listener.OnEventRaised(value);
            }
        }

        public void AddListener(IGenericEventListenerFI<T> newListener) {
            if (!listeners.Contains(newListener))
                listeners.Add(newListener);
        }

        public void RemoveListener(IGenericEventListenerFI<T> listenerToRemove) {
            if (listeners.Contains(listenerToRemove))
                listeners.Remove(listenerToRemove);
        }
    }
}