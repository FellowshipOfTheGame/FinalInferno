using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um eixo de itens.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Soft Change AIIState")]
    public class SoftChangeAIIState : ComponentRequester
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
            if(manager != null){
                manager.SetFocus(!manager.IsActive);
            }
        }
        
        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede um AIIManager.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            manager = provider.GetComponent<AIIManager>();
            // Debug.Log("Current manager for action " + name + " = " + provider.name);
        }

    }

}