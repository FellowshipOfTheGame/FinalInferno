using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Select Next Unit")]
    public class SelectNextUnit : Action {
        public override void Act(StateController controller) {
            BattleManager.instance.StartNextTurn();
        }
    }
}