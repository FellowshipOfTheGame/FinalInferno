using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Action that calls an animator trigger.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Trigger")]
    public class TriggerAction : ComponentRequester
    {
        /// <summary>
        /// Reference to the animator.
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Name of the parameter to be triggered.
        /// </summary>
        [SerializeField] private string trigger;

        /// <summary>
        /// Execute an action.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override void Act(StateController controller)
        {
            SetTrigger();
        }

        /// <summary>
        /// Set the trigger value.
        /// </summary>
        private void SetTrigger()
        {
            animator.SetTrigger(trigger);
        }

        /// <summary>
        /// Funcion called to request a component to the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        public override void RequestComponent(GameObject provider)
        {
            RequestAnimator(provider);
        }

        /// <summary>
        /// Request the animator component from the provider.
        /// </summary>
        /// <param name="provider"> Game object that provides the component requested. </param>
        private void RequestAnimator(GameObject provider)
        {
            animator = provider.GetComponent<Animator>();
        }
    }

}
