using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public abstract class GenericVariable<T> : ScriptableObject
    {
        [SerializeField] private T value;
        public T Value => value;

        public void UpdateValue(T newValue){
            value = newValue;
        }
    }
}