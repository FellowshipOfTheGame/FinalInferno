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
        private static GameObject particle = null;
        void UseSkill(){
            Debug.Log("Chamou o use skill pela animação");
            BattleSkillManager.currentSkill.Use(BattleSkillManager.currentUser, transform.parent.GetComponent<BattleUnit>());
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