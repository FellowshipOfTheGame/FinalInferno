using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Animation Ended")]
    public class AnimationEnded : Decision {
        private static bool animationEnded = false;
        private static bool isWaiting = false;

        public static void StartAnimation() {
            isWaiting = true;
        }

        public static void EndAnimation() {
            if (!isWaiting || animationEnded)
                return;
            animationEnded = true;
            isWaiting = false;
        }

        public override bool Decide(StateController controller) {
            if (animationEnded) {
                animationEnded = false;
                return true;
            }
            return false;
        }
    }
}
