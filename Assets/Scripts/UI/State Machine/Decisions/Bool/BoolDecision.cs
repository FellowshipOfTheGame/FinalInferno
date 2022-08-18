using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Bool")]
    public class BoolDecision : Decision {
        [SerializeField] private bool isTrue;
        [SerializeField] private BoolVariable variable = null;

        public override bool Decide(StateController controller) {
            return variable ? variable.Value : isTrue;
        }

        public void UpdateValue(bool newValue) {
            if (variable)
                variable.UpdateValue(newValue);
            else
                isTrue = newValue;
        }
    }
}
