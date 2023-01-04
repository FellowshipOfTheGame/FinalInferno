using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno.UI.AII {
    public class HorizontalSliderItem : AxisInteractableItem {
        public Slider slider;
        private bool active = false;
        [SerializeField] private float inputCooldown = 0.3f;
        [SerializeField] private InputActionReference movementAction;
        private float timer;

        public override void Awake() {
            base.Awake();
            OnEnter += Activate;
            OnExit += Deactivate;
            timer = 0f;
        }

        private void Activate() {
            active = true;
            timer = 0f;
        }

        private void Deactivate() {
            active = false;
        }

        private void Update() {
            if (!active)
                return;
            if (timer >= inputCooldown) {
                timer -= inputCooldown;
                MoveSliderToHorizontalInput();
            }
            timer += Time.deltaTime;
        }

        private void MoveSliderToHorizontalInput() {
            float input = movementAction.action.ReadValue<Vector2>().x;
            if (input == 0)
                return;
            float movementStep = 0.1f * (slider.maxValue - slider.minValue);
            slider.value += movementStep * input;
        }
    }
}