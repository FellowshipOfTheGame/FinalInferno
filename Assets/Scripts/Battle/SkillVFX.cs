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
        private static List<AudioClip> effectsPlaying = new List<AudioClip>();
        private List<GameObject> particleList = new List<GameObject>();
        private AudioSource src = null;
        [HideInInspector] public bool forceCallback = false;

        void Awake(){
            // Toca um efeito sonoro por skill
            src = GetComponent<AudioSource>();
            if(src != null && !effectsPlaying.Contains(src.clip)){
                effectsPlaying.Add(src.clip);
                src.Play();
            }else if(src != null){
                Destroy(src);
                src = null;
            }
        }

        void UseSkill(){
            if(!forceCallback){
                // Debug.Log("Chamou o use skill pela animação; " + "Object: " + gameObject.name);
                BattleSkillManager.currentSkill.Use(BattleSkillManager.currentUser, transform.parent.GetComponent<BattleUnit>());
            }
        }

        private void EndAnimation(){
            foreach(GameObject particle in particleList){
                if(particle != null){
                    Destroy(particle);
                }
            }

            if(src != null){
                effectsPlaying.Remove(src.clip);
            }

            Destroy(gameObject);
        }

        void DestroySkillObject()
        {
            if(!forceCallback){
                counter++;
                if(counter >= nTargets){
                    counter = 0;
                    nTargets = -1;

                    // Chama o callback de quando se usa a skill
                    // O usuario atual esta salvo como current user e os alvos da ultima skill estao em currenttargets
                    if(BattleSkillManager.currentUser.OnSkillUsed != null){
                        BattleSkillManager.currentUser.OnSkillUsed(BattleSkillManager.currentUser, BattleManager.instance.battleUnits);
                    }

                    FinalInferno.UI.FSM.AnimationEnded.EndAnimation();
                }
            }

            EndAnimation();
        }

        void CreateParticles(GameObject particles)
        {
            GameObject particle = Instantiate(particles, this.transform);
            particleList.Add(particle);
            ParticleSystemRenderer renderer = particle?.GetComponent<ParticleSystemRenderer>();
            if(renderer){
                renderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
                renderer.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
                renderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}