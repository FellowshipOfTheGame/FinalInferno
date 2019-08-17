using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;

namespace FinalInferno{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class SkillVFX : MonoBehaviour
    {
        public static int nTargets;
        private static int counter = 0;
        private GameObject particle = null;
        private static Transform canvasTransform = null;

        void Awake(){
            if(canvasTransform == null)
                canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
        }

        void Start(){
            transform.localScale = new Vector3(1.0f/canvasTransform.localScale.x,1.0f/canvasTransform.localScale.y,1.0f/canvasTransform.localScale.z);
        }

        void UseSkill(){
            Debug.Log("Chamou o use skill pela animação; " + "Object: " + gameObject.name);
            BattleSkillManager.currentSkill.Use(BattleSkillManager.currentUser, transform.parent.GetComponentInChildren<BattleUnit>());
        }

        void DestroySkillObject()
        {
            counter++;
            if(counter >= nTargets){
                counter = 0;
                nTargets = -1;
                BattleSkillManager.currentTargets.Clear();
                BattleSkillManager.currentSkill = null;
                BattleSkillManager.currentUser = null;
                FinalInferno.UI.FSM.AnimationEnded.EndAnimation();
            }

            if(particle != null)
                Destroy(particle);

            Destroy(gameObject);
        }

        void CreateParticles(GameObject particles)
        {
            particle = Instantiate(particles, new Vector3(transform.position.x, transform.position.y+((GetComponent<SpriteRenderer>()).size.y/2.0f), transform.position.z), transform.rotation);
        }
    }
}