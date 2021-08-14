using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno{
    public class FloatVariableObserver : MonoBehaviour, IVariableObserver<float>{
        [SerializeField] FloatVariable variable;
        [SerializeField] private UnityEvent<float> OnValueChanged;

        void OnEnable(){
            variable.AddObserver(this);
        }

        void OnDisable(){
            variable.RemoveObserver(this);
        }

		public void ValueChanged(float value){
            OnValueChanged?.Invoke(value);
		}
	}
}
