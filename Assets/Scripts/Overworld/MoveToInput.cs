using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno {
    public class MoveToInput : MoveTo {
        private Vector2 input;
        private bool isActive;
        [SerializeField] private InputActionReference movementAction;

        private void Start() {
            input = Vector2.zero;
        }

        public override void Activate() {
            input = Vector2.zero;
            isActive = true;
        }

        public override void Deactivate() {
            input = Vector2.zero;
            isActive = false;
        }

        override public Vector2 Direction() {
            return input.normalized;
        }

        private void Update() {
            if (isActive) {
                input = movementAction.action.ReadValue<Vector2>();
            }
        }
    }
}
