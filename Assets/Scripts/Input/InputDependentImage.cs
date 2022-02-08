using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno.Input {
    public class InputDependentImage : Image {
        [SerializeField] private InputActionAsset inputActions = null;
        private InputDevice lastDevice = null;
        [SerializeField] private List<string> controlSchemesNames = new List<string>();
        [SerializeField] private List<Sprite> controlSchemesImages = new List<Sprite>();

        protected override void OnEnable() {
            base.OnEnable();
            if (inputActions != null) {
                foreach (InputActionMap map in inputActions.actionMaps) {
                    map.actionTriggered += UpdateImage;
                }
            }
        }

        protected override void OnDisable() {
            if (inputActions != null) {
                foreach (InputActionMap map in inputActions.actionMaps) {
                    map.actionTriggered -= UpdateImage;
                }
            }
            base.OnDisable();
        }

        private void UpdateImage(InputAction.CallbackContext context) {
            InputDevice newDevice = context.control?.device;
            if (inputActions == null || newDevice == null || newDevice == lastDevice) {
                return;
            }

            lastDevice = newDevice;
            int index = controlSchemesNames.FindIndex(IsLastDeviceInControlScheme);
            if (index >= 0 && index < controlSchemesNames.Count) {
                if (controlSchemesImages[index] == null) {
                    Debug.LogError($"[InputDependentImage] No image set for mapping {controlSchemesNames[index]} in object {gameObject.name}");
                } else {
                    sprite = controlSchemesImages[index];
                }
            } else {
                Debug.LogWarning($"[InputDependentImage] Device {lastDevice.name} is not inside registered mappings for object {gameObject.name}");
            }
        }

        private bool IsLastDeviceInControlScheme(string schemeName) {
            InputControlScheme? controlScheme = inputActions.FindControlScheme(schemeName);
            return controlScheme?.SupportsDevice(lastDevice) ?? false;
        }
    }
}
