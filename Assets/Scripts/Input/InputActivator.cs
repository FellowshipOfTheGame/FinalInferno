using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno.Input{
    public class InputActivator : MonoBehaviour {
        [SerializeField] private InputActionAsset actions;

        void OnEnable() {
            actions.Enable();
        }

        void OnDisable() {
            actions.Disable();
        }
    }
}
