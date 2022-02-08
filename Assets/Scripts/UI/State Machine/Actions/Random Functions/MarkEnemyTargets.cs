using FinalInferno.UI.AII;
using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que marca os alvos da skill selecionada pelo inimigo.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Mark Enemy Targets")]
    public class MarkEnemyTargets : Action {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller) {
            foreach (BattleUnit battleUnit in BattleSkillManager.currentTargets) {
                battleUnit.battleItem.GetComponent<AxisInteractableItem>().EnableReference();
            }
        }

    }

}