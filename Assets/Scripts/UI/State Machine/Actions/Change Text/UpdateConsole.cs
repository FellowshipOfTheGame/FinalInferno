using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que atualiza o console com a ação que aconteceu esse turno.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Update Console")]
    public class UpdateConsole : Action {
        /// <summary>
        /// Executa uma ação.
        /// Chama o trigger.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            Console.Instance.UpdateConsole();
        }

    }

}