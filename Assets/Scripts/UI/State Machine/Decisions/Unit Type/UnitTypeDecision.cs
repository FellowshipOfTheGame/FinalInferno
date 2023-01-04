using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Unit Type")]
    public class UnitTypeDecision : Decision {
        [SerializeField] private UnitType desiredType;

        public override bool Decide(StateController controller) {
            return BattleManager.instance.GetCurrentUnitType() == desiredType;
        }
    }
}
