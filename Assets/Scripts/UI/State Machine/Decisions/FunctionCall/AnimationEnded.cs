using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Animation Ended")]
    public class AnimationEnded : Decision
    {
        private static bool animationEnded = false;
        public static bool isWaiting = false;
        
        public static void StartAnimation(){
            isWaiting = true;
        }
        public static void EndAnimation(){
            if(!animationEnded && isWaiting){
                animationEnded = true;
                isWaiting = false;
            }
        }

        public override bool Decide(StateController controller){
            if(animationEnded){
                animationEnded = false;
                return true;
            }
            return false;
        }
    }
}
