using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Update Targets")]
    public class UpdateTargets : Action {
        public override void Act(StateController controller) {
            BattleUnitsUI.Instance.UpdateTargetList();
        }
    }
}