using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Button State")]
    public class ChangeButtonStateAction : ComponentRequester {
        /// <summary>
        /// Referencia ao botão.
        /// </summary>
        private Button button;

        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            button.interactable = !button.interactable;
        }


        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Requisita um botão.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider) {
            button = provider.GetComponent<Button>();
        }

    }

}