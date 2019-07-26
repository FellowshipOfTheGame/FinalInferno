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
                FinalInferno.UI.FSM.AnimationEnded.EndAnimation();
            }
            Destroy(gameObject);
        }
    }
}