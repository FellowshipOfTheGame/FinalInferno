using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class GenericVariable<T> : ScriptableObject
    {
        [SerializeField] protected T value;
        public T Value => value;

        public void UpdateValue(T newValue){
            value = newValue;
        }
    }
}