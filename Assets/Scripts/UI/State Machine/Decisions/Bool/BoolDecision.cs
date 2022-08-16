using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Bool")]
    public class BoolDecision : Decision {
        [SerializeField] private bool isTrue;

        public override bool Decide(StateController controller) {
            return isTrue;
        }

        public void UpdateValue(bool newValue) {
            isTrue = newValue;
        }
    }
}
