using UnityEngine;

namespace FinalInferno {
    public class ParticlePlayer : MonoBehaviour {
        [SerializeField] private ParticleSystem particles;

        private void Reset() {
            if (!particles)
                particles = GetComponent<ParticleSystem>();
        }

        public void PlayParticles(bool withChildren = true) {
            if (particles)
                particles.Play(withChildren);
        }

        public void StopParticles(bool withChildren = true) {
            if (particles)
                particles.Stop(withChildren);
        }
    }
}
