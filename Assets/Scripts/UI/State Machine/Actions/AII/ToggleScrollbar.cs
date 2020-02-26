using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um eixo de itens.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Toggle Scrollbar")]
    public class ToggleScrollbar : ComponentRequester
    {
        [SerializeField] private KeyboardScrollbar scrollbar;

        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do gerenciador.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            scrollbar.Active = !(scrollbar.Active);
        }
        
        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede um AIIManager.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            scrollbar = provider.GetComponent<KeyboardScrollbar>();
        }

    }

}