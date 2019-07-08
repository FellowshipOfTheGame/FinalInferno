using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Animation Ended")]
    public class AnimationEnded : Decision
    {
        public static void StartAnimation(){
            isWaiting = true;
            nTargets = FinalInferno.UI.Battle.BattleSkillManager.currentTargets.Count;
        }
        public static void EndAnimation(){
            count++;
            if(!animationEnded && isWaiting && count >= nTargets){
                animationEnded = true;
                isWaiting = false;
            }
        }
        private static bool animationEnded = false;
        private static int nTargets;
        private static int count;
        public static bool isWaiting = false;

        public override bool Decide(StateController controller){
            if(animationEnded){
                animationEnded = false;
                return true;
            }
            return false;
        }
    }
}
