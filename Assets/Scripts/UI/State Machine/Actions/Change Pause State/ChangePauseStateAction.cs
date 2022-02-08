using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Pause State")]
    public class ChangePauseStateAction : Action {
        public override void Act(StateController controller) {
            PauseMenu.Instance.ChangePauseState();
        }

    }
}
