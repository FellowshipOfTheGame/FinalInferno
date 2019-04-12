using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Trigger")]
    public class TriggerAction : ComponentRequester
    {
        private Animator animator;
        [SerializeField] private string trigger;
        public override void Act(StateController controller)
        {
            SetTrigger(controller);
        }

        private void SetTrigger(StateController controller)
        {
            animator.SetTrigger(trigger);
        }

        public override void RequestComponent(GameObject provider)
        {
            RequestAnimator(provider);
        }

        private void RequestAnimator(GameObject provider)
        {
            animator = provider.GetComponent<Animator>();
        }
    }

}
