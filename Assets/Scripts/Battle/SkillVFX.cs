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
        private static AudioSource src = null;
        private static List<AudioClip> effectsPlaying = new List<AudioClip>();

        void Awake(){
            if(canvasTransform == null)
                canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;

            // Toca um efeito sonoro por skill
            AudioSource src = GetComponent<AudioSource>();
            if(!effectsPlaying.Contains(src.clip)){
                effectsPlaying.Add(src.clip);
                src.Play();
            }else{
                Destroy(src);
                src = null;
            }
        }

        void Start(){
            transform.localScale = new Vector3(1.0f/canvasTransform.localScale.x,1.0f/canvasTransform.localScale.y,1.0f/canvasTransform.localScale.z);
        }

        void UseSkill(){
            Debug.Log("Chamou o use skill pela animação; " + "Object: " + gameObject.name);
            BattleSkillManager.currentSkill.Use(BattleSkillManager.currentUser, transform.parent.GetComponentInChildren<BattleUnit>());
        }

        void EndAnimation(){
            if(particle != null){
                Destroy(particle);
            }

            if(src != null){
                effectsPlaying.Remove(src.clip);
            }

            Destroy(gameObject);
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

            EndAnimation();
        }

        void CreateParticles(GameObject particles)
        {
            particle = Instantiate(particles, new Vector3(transform.position.x, transform.position.y+((GetComponent<SpriteRenderer>()).size.y/2.0f), transform.position.z), transform.rotation);
        }
    }
}