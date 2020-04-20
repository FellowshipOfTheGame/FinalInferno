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
        private bool hasTurnsParameter = false;
        public int TurnsLeft {
            set{
                if(!anim){
                    anim = GetComponent<Animator>();
                    hasTurnsParameter = System.Array.Find(anim.parameters, param => param.name == "turnsLeft") != null;
                }
                if(hasTurnsParameter){
                    anim.SetInteger("turnsLeft", value);
                }
            }
        }

        public void Awake(){
            if(canvasTransform == null)
                canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
            if(!anim){
                anim = GetComponent<Animator>();
            }
            hasTurnsParameter = System.Array.Find(anim.parameters, param => param.name == "turnsLeft") != null;
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