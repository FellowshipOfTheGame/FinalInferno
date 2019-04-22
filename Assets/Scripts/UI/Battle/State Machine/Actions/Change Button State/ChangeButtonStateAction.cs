using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Action that change the state of a button.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Button State")]
    public class ChangeButtonStateAction : ComponentRequester
    {
        /// <summary>
        /// Reference to the button.
        /// </summary>
        private Button button;

        /// <summary>
        /// Execute an action.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override void Act(StateController controller)
        {
            ChangeButtonState();
        }

        /// <summary>
        /// Change the state of a button.
        /// </summary>
        private void ChangeButtonState()
        {
            button.interactable = !button.interactable;
        }

        /// <summary>
        /// Funcion called to request a component to the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        public override void RequestComponent(GameObject provider)
        {
            RequestButton(provider);
        }

        /// <summary>
        /// Request the button component from the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        private void RequestButton(GameObject provider)
        {
            button = provider.GetComponent<Button>();
        }

    }

}
