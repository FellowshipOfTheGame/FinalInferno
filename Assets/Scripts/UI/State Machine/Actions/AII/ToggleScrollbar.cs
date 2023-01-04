using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Toggle Scrollbar")]
    public class ToggleScrollbar : ComponentRequester {
        [SerializeField] private KeyboardScrollbar scrollbar;

        public override void Act(StateController controller) {
            scrollbar.Active = !scrollbar.Active;
        }

        public override void RequestComponent(GameObject provider) {
            scrollbar = provider.GetComponent<KeyboardScrollbar>();
        }
    }
}