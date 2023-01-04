using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class SkillVFX : MonoBehaviour {
        private const int sortingOrderOffset = 2;
        public static int nTargets;
        private static int counter = 0;
        private static List<AudioClip> effectsPlaying = new List<AudioClip>();

        [SerializeField] private TargetPosition spawnPosition = TargetPosition.Default;
        private List<GameObject> particleList = new List<GameObject>();
        [SerializeField] private AudioSource src = null;
        [SerializeField] private AudioClip clip = null;
        private bool isCallback = false;
        private SpriteRenderer spriteRenderer = null;
        [SerializeField] private BoolVariable skillAnimationStarted;
        [SerializeField] private BoolVariable skillAnimationEnded;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            CheckAudioSourceAndPlayAudio();
        }

        private void CheckAudioSourceAndPlayAudio() {
            if (!src) {
                src = null;
                return;
            } else if (clip == null || effectsPlaying.Contains(clip)) {
                Utils.DestroyIfExists(src);
                src = null;
                return;
            }
            src.clip = clip;
            effectsPlaying.Add(clip);
            src.Play();
        }

        public void SetTarget(BattleUnit unit, bool isCallback) {
            this.isCallback = isCallback;
            spriteRenderer.sortingOrder = unit.GetComponent<SpriteRenderer>().sortingOrder + sortingOrderOffset;
            UpdateLocalPosition(unit);
        }

        private void UpdateLocalPosition(BattleUnit unit) {
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
            if (isCallback)
                return;
            BattleSkillManager.CurrentSkill.Use(BattleSkillManager.CurrentUser, transform.parent.GetComponent<BattleUnit>());
        }

        private void DestroySkillObject() {
            if (!isCallback) {
                counter++;
                if (counter >= nTargets) {
                    counter = 0;
                    nTargets = -1;
                    BattleSkillManager.CurrentUser.OnSkillUsed?.Invoke(BattleSkillManager.CurrentUser, BattleManager.instance.battleUnits);
                    NotifySkillAnimationEnd();
                }
            }
            EndAnimation();
        }

        private void NotifySkillAnimationEnd() {
            if (skillAnimationStarted.Value && !skillAnimationEnded.Value) {
                skillAnimationStarted.UpdateValue(false);
                skillAnimationEnded.UpdateValue(true);
            }
        }

        private void EndAnimation() {
            foreach (GameObject particle in particleList) {
                Utils.DestroyIfExists(particle);
            }
            if (src)
                effectsPlaying.Remove(clip);
            Destroy(gameObject);
        }

        private void CreateParticles(GameObject particles) {
            GameObject particle = Instantiate(particles, transform.position, transform.rotation, transform);
            if (!particles)
                return;
            particleList.Add(particle);
            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (!renderer)
                return;
            renderer.sortingLayerID = spriteRenderer.sortingLayerID;
            renderer.sortingLayerName = spriteRenderer.sortingLayerName;
            renderer.sortingOrder = spriteRenderer.sortingOrder;
        }
    }
}