using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class ParticlePlayer : MonoBehaviour {
        [SerializeField] private ParticleSystem particles;

        void Reset(){
            if(particles == null){
                particles = GetComponent<ParticleSystem>();
            }
        }

        public void PlayParticles(bool withChildren = true){
            particles?.Play(withChildren);
        }

        public void StopParticles(bool withChildren = true){
            particles?.Stop(withChildren);
        }
    }
}
