using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Update Console")]
    public class UpdateConsole : Action {
        public override void Act(StateController controller) {
            Console.Instance.UpdateConsole();
        }
    }
}