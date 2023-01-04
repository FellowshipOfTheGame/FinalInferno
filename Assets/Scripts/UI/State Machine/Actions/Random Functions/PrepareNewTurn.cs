using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Prepare New Turn")]
    public class PrepareNewTurn : Action {
        public override void Act(StateController controller) {
            BattleSkillManager.BeginUnitTurn(BattleManager.instance.CurrentUnit);
        }
    }
}