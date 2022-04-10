using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public abstract class GenericVariable<T> : ScriptableObject {
        private List<IVariableObserver<T>> observers = new List<IVariableObserver<T>>();
        [SerializeField] protected T value;
        public T Value => value;

        public void AddObserver(IVariableObserver<T> obs) {
            if (!observers.Contains(obs)) {
                observers.Add(obs);
            }
        }

        public void RemoveObserver(IVariableObserver<T> obs) {
            if (observers.Contains(obs)) {
                observers.Remove(obs);
            }
        }

        public void UpdateValue(T newValue) {
            value = newValue;
            foreach (IVariableObserver<T> obs in observers.ToArray()) {
                if (obs != null)
                    obs.ValueChanged(value);
            }
        }
    }
}