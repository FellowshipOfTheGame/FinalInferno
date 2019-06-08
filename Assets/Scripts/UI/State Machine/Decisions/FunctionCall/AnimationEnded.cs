using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Animation Ended")]
    public class AnimationEnded : Decision
    {
        public static void EndAnimation(){
            if(!animationEnded && isWaiting){
                animationEnded = true;
                isWaiting = false;
            }
        }
        private static bool animationEnded = false;
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
