using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Trigger")]
    public class TriggerAction : ComponentRequester {
        private Animator animator;
        [SerializeField] private string trigger;

        public override void Act(StateController controller) {
            animator.SetTrigger(trigger);
        }

        public override void RequestComponent(GameObject provider) {
            animator = provider.GetComponent<Animator>();
        }
    }
}
