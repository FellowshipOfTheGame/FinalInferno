using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Text")]
    public class ChangeTextAction : ComponentRequester {
        private Text text;
        [SerializeField] private string newText;

        public override void Act(StateController controller) {
            text.text = newText;
        }

        public override void RequestComponent(GameObject provider) {
            text = provider.GetComponent<Text>();
        }
    }
}
