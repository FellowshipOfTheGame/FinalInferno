using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que altera o texto de um Text.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Text")]
    public class ChangeTextAction : ComponentRequester {
        /// <summary>
        /// Referência ao Text.
        /// </summary>
        private Text text;

        /// <summary>
        /// Novo texto.
        /// </summary>
        [SerializeField] private string newText;

        /// <summary>
        /// Executa uma ação.
        /// Chama o trigger.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            text.text = newText;
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Requisita um Text.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider) {
            text = provider.GetComponent<Text>();
        }

    }

}
