using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que atualiza a lista de items selecionaveis.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Update Targets")]
    public class UpdateTargets : Action {
        /// <summary>
        /// Executa uma ação.
        /// Usa a skill selecionada atualmente.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            BattleUnitsUI.Instance.UpdateTargetList();
        }
    }

}