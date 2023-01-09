using System.Collections;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Automatic Action End")]
    public class AutomaticActionEnd : Action {
        [SerializeField] private float delay = 0f;
        [SerializeField] private BoolVariable skillAnimationStarted;
        [SerializeField] private BoolVariable skillAnimationEnded;

        public override void Act(StateController controller) {
            skillAnimationStarted.UpdateValue(true);
            BattleManager.instance.StartCoroutine(EndAnimationAfterSeconds(delay));
        }

        private IEnumerator EndAnimationAfterSeconds(float time) {
            yield return new WaitForSeconds(time);
            NotifySkillAnimationEnd();
        }

        private void NotifySkillAnimationEnd() {
            if (skillAnimationStarted.Value && !skillAnimationEnded.Value) {
                skillAnimationStarted.UpdateValue(false);
                skillAnimationEnded.UpdateValue(true);
            }
        }
    }
}