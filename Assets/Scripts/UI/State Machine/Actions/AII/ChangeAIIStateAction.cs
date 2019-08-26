using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um eixo de itens.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change AII State")]
    public class ChangeAIIStateAction : ComponentRequester
    {
        /// <summary>
        /// Referência ao gerenciador do eixo.
        /// </summary>
        [SerializeField] private AIIManager manager;

        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do gerenciador.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            if (manager.active)
                manager.Desactive();
            else
                manager.Active();
        }
        
        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede um AIIManager.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            manager = provider.GetComponent<AIIManager>();
        }

    }

}