using System.Collections;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Automatic Action End")]
    public class AutomaticActionEnd : Action {
        [SerializeField] private float delay = 0f;

        public override void Act(StateController controller) {
            AnimationEnded.StartAnimation();
            BattleManager.instance.StartCoroutine(EndAnimationAfterSeconds(delay));
        }

        private IEnumerator EndAnimationAfterSeconds(float time) {
            yield return new WaitForSeconds(time);
            AnimationEnded.EndAnimation();
        }
    }
}