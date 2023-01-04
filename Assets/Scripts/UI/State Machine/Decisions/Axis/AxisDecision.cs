using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Axis")]
    public class AxisDecision : Decision {
        [SerializeField] private InputActionReference buttonAction;
        public override bool Decide(StateController controller) {
            return buttonAction.action.triggered;
        }
    }
}
