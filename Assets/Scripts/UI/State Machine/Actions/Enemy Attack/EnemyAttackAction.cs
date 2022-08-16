using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Enemy Attack")]
    public class EnemyAttackAction : Action {
        public override void Act(StateController controller) {
            Enemy currentEnemy = (Enemy)BattleManager.instance.CurrentUnit.Unit;
            currentEnemy.AIEnemy();
        }
    }
}