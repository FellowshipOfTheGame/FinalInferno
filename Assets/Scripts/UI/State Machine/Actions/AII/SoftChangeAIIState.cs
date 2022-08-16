using FinalInferno.UI.AII;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Soft Change AIIState")]
    public class SoftChangeAIIState : ComponentRequester {
        [SerializeField] private AIIManager manager;

        public override void Act(StateController controller) {
            manager.SetFocus(!manager.IsActive);
        }

        public override void RequestComponent(GameObject provider) {
            manager = provider.GetComponent<AIIManager>();
        }
    }
}