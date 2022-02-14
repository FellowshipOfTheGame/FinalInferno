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
                AddActionCallbacks();
            }
        }

        private void AddActionCallbacks() {
            foreach (InputActionMap map in inputActions.actionMaps) {
                map.actionTriggered += DetectDeviceAndUpdateImage;
            }
        }

        protected override void OnDisable() {
            if (inputActions != null) {
                RemoveActionCallbacks();
            }
            base.OnDisable();
        }

        private void RemoveActionCallbacks() {
            foreach (InputActionMap map in inputActions.actionMaps) {
                map.actionTriggered -= DetectDeviceAndUpdateImage;
            }
        }

        private void DetectDeviceAndUpdateImage(InputAction.CallbackContext context) {
            InputDevice newDevice = context.control?.device;
            if (ShouldIgnoreChange(newDevice)) {
                return;
            }
            lastDevice = newDevice;
            UpdateImage();
        }

        private bool ShouldIgnoreChange(InputDevice newDevice) {
            return inputActions == null || newDevice == null || newDevice == lastDevice;
        }

        private void UpdateImage() {
            int deviceIndex = controlSchemesNames.FindIndex(IsLastDeviceInControlScheme);
            if (deviceIndex < 0 || deviceIndex >= controlSchemesNames.Count) {
                LogUnregisteredDeviceWarning();
                return;
            }
            UpdateImageSprite(deviceIndex);
        }

        private void LogUnregisteredDeviceWarning() {
            Debug.LogWarning($"[InputDependentImage] Device {lastDevice.name} is not inside registered mappings for object {gameObject.name}");
        }

        private void UpdateImageSprite(int deviceIndex) {
            if (controlSchemesImages[deviceIndex] == null) {
                LogNullImageError(deviceIndex);
            } else {
                sprite = controlSchemesImages[deviceIndex];
            }
        }

        private void LogNullImageError(int index) {
            Debug.LogError($"[InputDependentImage] No image set for mapping {controlSchemesNames[index]} in object {gameObject.name}");
        }

        private bool IsLastDeviceInControlScheme(string schemeName) {
            InputControlScheme? controlScheme = inputActions.FindControlScheme(schemeName);
            return controlScheme?.SupportsDevice(lastDevice) ?? false;
        }
    }
}
