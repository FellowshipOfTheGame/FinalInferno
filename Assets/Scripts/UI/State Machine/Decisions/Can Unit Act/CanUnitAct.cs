using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Unit Act")]
    public class CanUnitAct : Decision {
        [SerializeField] private bool canAct;

        public override bool Decide(StateController controller) {
            return canAct == (BattleManager.instance.CurrentUnit != null && BattleManager.instance.CurrentUnit.CanAct);
        }
    }
}
