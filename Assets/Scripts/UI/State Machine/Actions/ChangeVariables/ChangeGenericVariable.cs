using UnityEngine;

namespace FinalInferno.UI.FSM {
    public class ChangeGenericVariable<T, U> : Action where T : GenericVariable<U> {
        [SerializeField] private T variable;
        [SerializeField] private U value;
        public override void Act(StateController controller) {
            if (variable != null) {
                variable.UpdateValue(value);
            }
        }
    }
}