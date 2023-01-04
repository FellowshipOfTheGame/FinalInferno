using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Animation Ended")]
    public class AnimationEnded : Decision {
        [SerializeField] private BoolVariable skillAnimationEnded;

        public override bool Decide(StateController controller) {
            if (skillAnimationEnded.Value) {
                skillAnimationEnded.UpdateValue(false);
                return true;
            }
            return false;
        }
    }
}
