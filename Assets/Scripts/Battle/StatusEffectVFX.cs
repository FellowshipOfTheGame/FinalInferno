using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class StatusEffectVFX : MonoBehaviour
    {
        private static Transform canvasTransform = null;
        private GameObject particle = null;
        private Animator anim = null;
        public int TurnsLeft {
            set{
                if(!anim){
                    anim = GetComponent<Animator>();
                }
                foreach (AnimatorControllerParameter parameter in anim.parameters)
                {
                    if(parameter.name == "turnsLeft"){
                        anim.SetInteger("turnsLeft", value);
                        break;
                    }
                }
            }
        }

        public void Awake(){
            if(canvasTransform == null)
                canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
        }

        public void Start(){
            transform.localScale = new Vector3(1.0f/canvasTransform.localScale.x,1.0f/canvasTransform.localScale.y,1.0f/canvasTransform.localScale.z);
        }

        public void DestroyVFX(){
            if(particle){
                Destroy(particle);
            }
            Destroy(gameObject);
        }

        void CreateParticles(GameObject particles)
        {
            particle = Instantiate(particles, new Vector3(transform.position.x, transform.position.y+((GetComponent<SpriteRenderer>()).size.y/2.0f), transform.position.z), transform.rotation, this.transform);
        }
    }
}