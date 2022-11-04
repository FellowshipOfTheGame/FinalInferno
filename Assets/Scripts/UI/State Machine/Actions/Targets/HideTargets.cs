using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Hide Targets")]
    public class HideTargets : Action {
        public override void Act(StateController controller) {
            foreach (BattleUnit target in BattleSkillManager.CurrentTargets) {
                target.StopShowingThisAsATarget();
            }
        }
    }
}