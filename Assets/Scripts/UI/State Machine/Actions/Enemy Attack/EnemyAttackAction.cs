﻿using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Enemy Attack")]
    public class EnemyAttackAction : Action {
        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            Enemy currentEnemy = (Enemy)BattleManager.instance.CurrentUnit.Unit;
            currentEnemy.AIEnemy();
        }

    }

}