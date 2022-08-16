using UnityEngine;

namespace FinalInferno.UI.FSM {
    public class StateController : MonoBehaviour {
        [SerializeField] private State currentState;
        private State nextState = null;
        private Action[] changeActions;
        private float stateTimeElapsed;
        [SerializeField] private Action[] startActions;

        private void Start() {
            foreach (Action action in startActions) {
                action.Act(this);
            }
            nextState = null;
        }

        private void Update() {
            if (nextState != null) {
                TransitionToNextState();
            } else {
                stateTimeElapsed += Time.deltaTime;
                currentState.CheckTransitions(this);
            }
        }

        private void TransitionToNextState() {
            stateTimeElapsed = 0;
            ExecuteTransitionActions(changeActions);
            currentState = nextState;
            nextState = null;
        }

        private void ExecuteTransitionActions(Action[] transitionActions) {
            foreach (Action action in transitionActions) {
                action.Act(this);
            }
        }

        public void SetNextState(State state, Action[] actions) {
            nextState = state;
            changeActions = actions;
        }

        public bool CheckIfCountDownElapsed(float duration) {
            return stateTimeElapsed >= duration;
        }
    }
}
