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
            if (active) {
                if (timer >= inputCooldown) {
                    timer -= inputCooldown;
                    // float input = UnityEngine.Input.GetAxisRaw("Horizontal");
                    float input = movementAction.action.ReadValue<Vector2>().x;
                    if (input != 0) {
                        // Move o slider 10% para a direita ou para a esquerda
                        slider.value += 0.1f * (slider.maxValue - slider.minValue) * input;
                    }
                }
                timer += Time.deltaTime;
            }
        }
    }
}