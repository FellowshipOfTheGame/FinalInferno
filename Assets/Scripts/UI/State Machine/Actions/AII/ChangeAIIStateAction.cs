using FinalInferno.UI.AII;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change AII State")]
    public class ChangeAIIStateAction : ComponentRequester {
        [SerializeField] private AIIManager manager;

        public override void Act(StateController controller) {
            manager.ToggleActive();
        }

        public override void RequestComponent(GameObject provider) {
            manager = provider.GetComponent<AIIManager>();
        }
    }
}