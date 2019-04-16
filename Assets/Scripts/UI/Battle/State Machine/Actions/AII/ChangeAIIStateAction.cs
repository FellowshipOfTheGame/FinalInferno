using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Action that change the state of an AII.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change AII State")]
    public class ChangeAIIStateAction : ComponentRequester
    {
        /// <summary>
        /// Reference to the AII manager.
        /// </summary>
        private AIIManager manager;

        /// <summary>
        /// Execute an action.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override void Act(StateController controller)
        {
            ChangeAIIState();
        }

        /// <summary>
        /// Change the state of an AII manager.
        /// </summary>
        private void ChangeAIIState()
        {
            if (manager.active)
                manager.Desactive();
            else
                manager.Active();
        }

        /// <summary>
        /// Funcion called to request a component to the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        public override void RequestComponent(GameObject provider)
        {
            RequestAIIManager(provider);
        }

        /// <summary>
        /// Request the AII manager component from the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        private void RequestAIIManager(GameObject provider)
        {
            manager = provider.GetComponent<AIIManager>();
        }
    }

}