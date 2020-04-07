using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.AII{
    public class HorizontalSliderItem : AxisInteractableItem
    {
        public Slider slider;
        private bool active = false;
        [SerializeField] private float inputCooldown = 0.3f;
        private float timer;

        public override void Awake(){
            OnEnter += EnableReference;
            OnEnter += Activate;
            OnExit += DisableReference;
            OnExit += Deactivate;
            timer = 0f;
        }

        void Activate(){
           active = true;
           timer = 0f;
        }

        void Deactivate(){
            active = false;
        }

        void Update(){
            if(active){
                if(timer >= inputCooldown){
                    timer -= inputCooldown;
                    float input = Input.GetAxisRaw("Horizontal");
                    if(input != 0){
                        // Move o slider 10% para a direita ou para a esquerda
                        slider.value += 0.1f * (slider.maxValue - slider.minValue) * input;
                    }
                }
                timer += Time.deltaTime;
            }
        }
    }
}