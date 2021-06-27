using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno{
    public class SprintSkillButton : MonoBehaviour
    {
        [SerializeField] OverworldSkill sprintSkill = null;
        [SerializeField] private InputActionReference buttonAction;
        private bool isButtonDown = false;

        private float movespeedIncrease => sprintSkill?.effects[0].value1 ?? 0;

        void Start(){
            if(sprintSkill == null || sprintSkill.Level < 1){
                gameObject.SetActive(false);
            }
        }

        void OnEnable(){
            isButtonDown = false;
            buttonAction.action.performed += SetButtonDown;
            buttonAction.action.canceled += SetButtonUp;
        }

        void OnDisable(){
            buttonAction.action.performed -= SetButtonDown;
            buttonAction.action.canceled -= SetButtonUp;
            isButtonDown = false;
        }

        private void SetButtonDown(InputAction.CallbackContext context){
            isButtonDown = true;
        }
        private void SetButtonUp(InputAction.CallbackContext context){
            isButtonDown = false;
        }

        void Update(){
            if(!sprintSkill.active){
                if(CharacterOW.PartyCanMove && isButtonDown){
                    sprintSkill?.Activate();
                }
            }else{
                if(CharacterOW.PartyCanMove && !isButtonDown){
                    sprintSkill?.Deactivate();
                }
            }
        }
    }
}
