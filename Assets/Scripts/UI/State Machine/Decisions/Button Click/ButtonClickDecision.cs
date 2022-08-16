using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Button Click")]
    public class ButtonClickDecision : Decision {
        private bool buttonIsClicked = false;

        public override bool Decide(StateController controller) {
            bool clickedThisFrame = buttonIsClicked;
            buttonIsClicked = false;
            return clickedThisFrame;
        }

        public void Click() {
            buttonIsClicked = true;
        }
    }

}
