using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno {
    public class FloatVariableObserver : MonoBehaviour, IVariableObserver<float> {
        [SerializeField] private FloatVariable variable;
        [SerializeField] private UnityEvent<float> OnValueChanged;

        private void OnEnable() {
            variable.AddObserver(this);
        }

        private void OnDisable() {
            variable.RemoveObserver(this);
        }

        public void ValueChanged(float value) {
            OnValueChanged?.Invoke(value);
        }
    }
}
