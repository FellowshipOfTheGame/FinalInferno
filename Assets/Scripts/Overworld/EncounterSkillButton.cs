using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno{
    public class EncounterSkillButton : MonoBehaviour, IVariableObserver<float>
    {
        [SerializeField] OverworldSkill encounterSkill = null;
        [SerializeField] FloatVariable distanceWalkedRef;
        private float DistanceWalked => distanceWalkedRef?.Value ?? 0;
        [SerializeField] private float skillCooldownDistance = 5f;
        [SerializeField] private string buttonString = "";
        [SerializeField] private Image fillImage;

        private float skillDistance = 0;
        private bool onCooldown = false;

        void Start(){
            if(encounterSkill == null || encounterSkill.Level < 1){
                gameObject.SetActive(false);
            }else{
                skillDistance = encounterSkill?.effects[0].value2 ?? 0;
                onCooldown = false;
                skillCooldownDistance = Mathf.Max(skillCooldownDistance, float.Epsilon);
            }
        }

        void Update(){
            if(!encounterSkill.active){
                if(CharacterOW.PartyCanMove && !onCooldown && Input.GetButtonDown(buttonString)){
                    encounterSkill.Activate();
                    onCooldown = true;
                }
            }
        }

        void OnEnable(){
            distanceWalkedRef.AddObserver(this);
        }

        void OnDisable(){
            distanceWalkedRef.RemoveObserver(this);
        }

		public void ValueChanged(float value){
            if(!encounterSkill.active){
                if(onCooldown){
                    float cooldown = Mathf.Max((DistanceWalked - skillDistance), 0) / skillCooldownDistance;
                    if(cooldown >= 1.0f){
                        onCooldown = false;
                    }
                    cooldown = Mathf.Clamp(cooldown, 0, 1.0f);
                    fillImage.fillAmount = cooldown;
                }else{
                    fillImage.fillAmount = 1.0f;
                }
            }else{
                fillImage.fillAmount = 1.0f;
                if(DistanceWalked > skillDistance){
                    encounterSkill.Deactivate();
                }
            }
		}
	}
}
