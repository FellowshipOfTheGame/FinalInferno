using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Button State")]
    public class ChangeButtonStateAction : ComponentRequester
    {
        private Button button;
        public override void Act(StateController controller)
        {
            ChangeButtonState(controller);
        }

        private void ChangeButtonState(StateController controller)
        {
            button.interactable = !button.interactable;
        }

        public override void RequestComponent(GameObject provider)
        {
            RequestButton(provider);
        }

        private void RequestButton(GameObject provider)
        {
            button = provider.GetComponent<Button>();
        }

    }

}
