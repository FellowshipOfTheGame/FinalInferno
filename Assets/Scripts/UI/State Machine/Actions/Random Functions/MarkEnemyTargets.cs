using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Mark Enemy Targets")]
    public class MarkEnemyTargets : Action {
        public override void Act(StateController controller) {
            foreach (BattleUnit battleUnit in BattleSkillManager.CurrentTargets) {
                battleUnit.OnUnitSelected?.Invoke();
            }
        }
    }
}