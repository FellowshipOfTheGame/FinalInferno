using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class SprintSkillButton : MonoBehaviour
    {
        [SerializeField] OverworldSkill sprintSkill = null;
        [SerializeField] private string buttonString = "";

        private float movespeedIncrease => sprintSkill?.effects[0].value1 ?? 0;

        void Start(){
            if(sprintSkill == null || sprintSkill.Level < 1){
                gameObject.SetActive(false);
            }
        }

        void Update(){
            if(!sprintSkill.active){
                if(CharacterOW.PartyCanMove && Input.GetButton(buttonString)){
                    sprintSkill?.Activate();
                }
            }else{
                if(CharacterOW.PartyCanMove && !Input.GetButton(buttonString)){
                    sprintSkill?.Deactivate();
                }
            }
        }
    }
}
