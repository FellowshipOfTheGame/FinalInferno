using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Wait For Skill To End")]
    public class WaitForSkillAnimation : Action {
        [SerializeField] private BoolVariable skillAnimationStarted;
        public override void Act(StateController controller) {
            skillAnimationStarted.UpdateValue(true);
        }
    }
}