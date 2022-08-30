using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno {
    public class EncounterSkillButton : MonoBehaviour, IVariableObserver<float> {
        [SerializeField] private OverworldSkill encounterSkill = null;
        [SerializeField] private FloatVariable distanceWalkedRef;
        private float DistanceWalked => distanceWalkedRef ? distanceWalkedRef.Value : 0;
        [SerializeField] private float skillCooldownDistance = 5f;
        [SerializeField] private InputActionReference buttonAction;
        private bool isButtonDown = false;
        [SerializeField] private Image fillImage;

        private float skillDistance = 0;
        private bool onCooldown = false;

        private void Start() {
            if (!encounterSkill || encounterSkill.Level < 1) {
                gameObject.SetActive(false);
            } else {
                ResetValuesToDefault();
            }
        }

        private void ResetValuesToDefault() {
            skillDistance = encounterSkill ? encounterSkill.effects[0].value2 : 0;
            onCooldown = false;
            skillCooldownDistance = Mathf.Max(skillCooldownDistance, float.Epsilon);
        }

        private void Update() {
            if (!encounterSkill.active) {
                CheckSkillActivation();
            }
        }

        private void CheckSkillActivation() {
            if (!CharacterOW.PartyCanMove || onCooldown || !isButtonDown) {
                return;
            }
            encounterSkill.Activate();
            onCooldown = true;
        }

        private void OnEnable() {
            SetupCallbacks();
            isButtonDown = false;
        }

        private void SetupCallbacks() {
            distanceWalkedRef.AddObserver(this);
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
            distanceWalkedRef.RemoveObserver(this);
            buttonAction.action.performed -= SetButtonDown;
            buttonAction.action.canceled -= SetButtonUp;
        }

        public void ValueChanged(float value) {
            if (encounterSkill.active) {
                fillImage.fillAmount = 1.0f;
                DeactivateOnMovementLimit();
            } else {
                DisplayCooldownStatus();
            }
        }

        private void DisplayCooldownStatus() {
            float cooldown = GetCooldownValue();
            onCooldown = cooldown < 1.0f;
            fillImage.fillAmount = Mathf.Clamp(cooldown, 0, 1.0f);
        }

        private float GetCooldownValue() {
            if (!onCooldown) {
                return 1.0f;
            }
            return Mathf.Max((DistanceWalked - skillDistance), 0) / skillCooldownDistance;
        }

        private void DeactivateOnMovementLimit() {
            if (DistanceWalked > skillDistance) {
                encounterSkill.Deactivate();
            }
        }
    }
}
