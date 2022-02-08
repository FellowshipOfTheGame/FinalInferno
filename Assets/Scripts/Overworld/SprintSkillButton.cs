using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno {
    public class SprintSkillButton : MonoBehaviour {
        [SerializeField] private OverworldSkill sprintSkill = null;
        [SerializeField] private InputActionReference buttonAction;
        private bool isButtonDown = false;

        private float movespeedIncrease => sprintSkill?.effects[0].value1 ?? 0;

        private void Start() {
            if (sprintSkill == null || sprintSkill.Level < 1) {
                gameObject.SetActive(false);
            } else if (sprintSkill != null) {
                sprintSkill.active = false;
            }
        }

        private void OnEnable() {
            isButtonDown = false;
            buttonAction.action.performed += SetButtonDown;
            buttonAction.action.canceled += SetButtonUp;
        }

        private void OnDisable() {
            buttonAction.action.performed -= SetButtonDown;
            buttonAction.action.canceled -= SetButtonUp;
            isButtonDown = false;
        }

        private void SetButtonDown(InputAction.CallbackContext context) {
            isButtonDown = true;
        }
        private void SetButtonUp(InputAction.CallbackContext context) {
            isButtonDown = false;
        }

        private void Update() {
            if (!sprintSkill.active) {
                if (CharacterOW.PartyCanMove && isButtonDown) {
                    sprintSkill?.Activate();
                }
            } else {
                if (CharacterOW.PartyCanMove && !isButtonDown) {
                    sprintSkill?.Deactivate();
                }
            }
        }
    }
}
