using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Show Targets")]
    public class ShowTargets : Action {
        public override void Act(StateController controller) {
            foreach (BattleUnit target in BattleSkillManager.CurrentTargets) {
                target.ShowThisAsATarget();
            }
        }
    }
}