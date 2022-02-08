using System.Collections.Generic;
using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class SkillVFX : MonoBehaviour {
        public enum TargetPosition {
            Default = 0,
            Feet,
            Torso,
            Head,
            Overhead
        }
        public static int nTargets;
        private static int counter = 0;
        private static List<AudioClip> effectsPlaying = new List<AudioClip>();

        [SerializeField] private TargetPosition spawnPosition = TargetPosition.Default;
        private List<GameObject> particleList = new List<GameObject>();
        private AudioSource src = null;
        [HideInInspector] public bool forceCallback = false;

        private void Awake() {
            // Toca um efeito sonoro por skill
            src = GetComponent<AudioSource>();
            if (src != null && !effectsPlaying.Contains(src.clip)) {
                effectsPlaying.Add(src.clip);
                src.Play();
            } else if (src != null) {
                Destroy(src);
                src = null;
            }
        }

        public void SetTarget(BattleUnit unit, bool isCallback = false) {
            forceCallback = isCallback;
            GetComponent<SpriteRenderer>().sortingOrder = unit.GetComponent<SpriteRenderer>().sortingOrder + 2;
            switch (spawnPosition) {
                case TargetPosition.Default:
                    transform.localPosition = unit.DefaultSkillPosition;
                    break;
                case TargetPosition.Feet:
                    transform.localPosition = unit.FeetPosition;
                    break;
                case TargetPosition.Torso:
                    transform.localPosition = unit.TorsoPosition;
                    break;
                case TargetPosition.Head:
                    transform.localPosition = unit.HeadPosition;
                    break;
                case TargetPosition.Overhead:
                    transform.localPosition = unit.OverheadPosition;
                    break;
            }
        }

        private void UseSkill() {
            if (!forceCallback) {
                // Debug.Log("Chamou o use skill pela animação; " + "Object: " + gameObject.name);
                BattleSkillManager.currentSkill.Use(BattleSkillManager.currentUser, transform.parent.GetComponent<BattleUnit>());
            }
        }

        private void EndAnimation() {
            foreach (GameObject particle in particleList) {
                if (particle != null) {
                    Destroy(particle);
                }
            }

            if (src != null) {
                effectsPlaying.Remove(src.clip);
            }

            Destroy(gameObject);
        }

        private void DestroySkillObject() {
            if (!forceCallback) {
                counter++;
                if (counter >= nTargets) {
                    counter = 0;
                    nTargets = -1;

                    // Chama o callback de quando se usa a skill
                    // O usuario atual esta salvo como current user e os alvos da ultima skill estao em currenttargets
                    if (BattleSkillManager.currentUser.OnSkillUsed != null) {
                        BattleSkillManager.currentUser.OnSkillUsed(BattleSkillManager.currentUser, BattleManager.instance.battleUnits);
                    }

                    FinalInferno.UI.FSM.AnimationEnded.EndAnimation();
                }
            }

            EndAnimation();
        }

        private void CreateParticles(GameObject particles) {
            GameObject particle = Instantiate(particles, transform.position, transform.rotation, transform);
            particleList.Add(particle);
            ParticleSystemRenderer renderer = particle?.GetComponent<ParticleSystemRenderer>();
            if (renderer) {
                renderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
                renderer.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
                renderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}