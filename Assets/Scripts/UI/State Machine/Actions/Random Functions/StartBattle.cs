using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Start Battle")]
    public class StartBattle : Action {
        public override void Act(StateController controller) {
            BattleManager.instance.StartBattle();
        }
    }
}