using FinalInferno.UI.AII;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que muda o estado de um eixo de itens.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change AII State")]
    public class ChangeAIIStateAction : ComponentRequester {
        /// <summary>
        /// Referência ao gerenciador do eixo.
        /// </summary>
        [SerializeField] private AIIManager manager;

        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do gerenciador.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            if (manager != null && manager.IsActive) {
                manager.Deactivate();
            } else if (manager != null) {
                manager.Activate();
            }
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede um AIIManager.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider) {
            manager = provider.GetComponent<AIIManager>();
        }

    }

}