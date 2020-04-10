using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace FinalInferno.UI.AII{
    public class ToggleItem : AxisInteractableItem
    {
        [Header("Toggle")]
        [SerializeField] private GameObject hideObject;
        [SerializeField] private TextMeshProUGUI textIndicator;
        [SerializeField] private Image imageIndicator;
        [SerializeField] private float inputCooldown = 0.15f;
        public UnityAction OnToggle = null;
        private float timer;
        private bool active;
        private bool isOn = false;

        public override void Awake(){
            base.Awake();
            OnEnter += Activate;
            OnExit += Deactivate;
            OnAct += Toggle;
            timer = 0f;
            active = false;
        }

        void Activate(){
           active = true;
           timer = -inputCooldown;
        }

        void Deactivate(){
            active = false;
        }

        void Update(){
            if(active){
                if(timer < inputCooldown){
                    timer += Time.deltaTime;
                }
            }
        }

        public void Show(){
            if(hideObject){
                hideObject.SetActive(true);
            }
        }

        public void Hide(){
            if(hideObject){
                hideObject.SetActive(false);
            }
        }

        private void Toggle(){
            if(timer < inputCooldown)
                return;
            timer = 0f;
            Toggle(null);
            OnToggle?.Invoke();
        }

        public void Toggle(bool? value = null){

            isOn = value ?? !isOn;
            if(textIndicator){
                textIndicator.text = (isOn)? "<color=green>On</color>" : "<color=red>Off</color>";
            }
            if(imageIndicator){
                imageIndicator.enabled = isOn;
            }
        }
    }
}