using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/State")]
    public class State : ScriptableObject {
        public Transition[] transitions;

        public void CheckTransitions(StateController controller) {
            CheckForNextState(controller);
            ResetDecisions();
        }

        private void CheckForNextState(StateController controller) {
            foreach (Transition transition in transitions) {
                bool decisionSucceeded = true;
                foreach (Decision decision in transition.decisions) {
                    decisionSucceeded &= decision.Decide(controller);
                }
                if (decisionSucceeded)
                    controller.SetNextState(transition.nextState, transition.actions);
            }
        }

        private void ResetDecisions() {
            foreach (Transition transition in transitions) {
                foreach (Decision decision in transition.decisions) {
                    decision.Reset();
                }
            }
        }
    }
}
