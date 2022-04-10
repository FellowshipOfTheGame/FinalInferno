using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno {
    public class SprintSkillButton : MonoBehaviour {
        [SerializeField] private OverworldSkill sprintSkill = null;
        [SerializeField] private InputActionReference buttonAction;
        private bool isButtonDown = false;

        private float MovespeedIncrease => sprintSkill ? sprintSkill.effects[0].value1 : 0;

        private void Start() {
            if (!sprintSkill || sprintSkill.Level < 1) {
                gameObject.SetActive(false);
            } else if (sprintSkill) {
                sprintSkill.active = false;
            }
        }

        private void OnEnable() {
            isButtonDown = false;
            SetupCallbacks();
        }

        private void SetupCallbacks() {
            buttonAction.action.performed += SetButtonDown;
            buttonAction.action.canceled += SetButtonUp;
        }

        private void SetButtonDown(InputAction.CallbackContext context) {
            isButtonDown = true;
        }
        private void SetButtonUp(InputAction.CallbackContext context) {
            isButtonDown = false;
        }

        private void OnDisable() {
            RemoveCallbacks();
            isButtonDown = false;
        }

        private void RemoveCallbacks() {
            buttonAction.action.performed -= SetButtonDown;
            buttonAction.action.canceled -= SetButtonUp;
        }

        private void Update() {
            if (!sprintSkill.active) {
                CheckButtonPress();
            } else {
                CheckButtonRelease();
            }
        }

        private void CheckButtonPress() {
            if (CharacterOW.PartyCanMove && isButtonDown && sprintSkill) {
                sprintSkill.Activate();
            }
        }

        private void CheckButtonRelease() {
            if (CharacterOW.PartyCanMove && !isButtonDown && sprintSkill) {
                sprintSkill.Deactivate();
            }
        }
    }
}
