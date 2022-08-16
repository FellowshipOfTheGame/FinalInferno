using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Button State")]
    public class ChangeButtonStateAction : ComponentRequester {
        private Button button;

        public override void Act(StateController controller) {
            button.interactable = !button.interactable;
        }

        public override void RequestComponent(GameObject provider) {
            button = provider.GetComponent<Button>();
        }
    }
}