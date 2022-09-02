using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que atualiza o turno, selecionando a nova unidade atual.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Select Next Unit")]
    public class SelectNextUnit : Action {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller) {
            BattleManager.instance.StartNextTurn();
        }

    }

}