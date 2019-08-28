using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decisão baseada na ativação de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Button Click")]
    public class ButtonClickDecision : Decision
    {
        /// <summary>
        /// Variável que checa se o botão foi clicado ou não.
        /// </summary>
        [SerializeField] private bool buttonIsClicked;

        /// <summary>
        /// Verifica se a decisão ativou.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller)
        {
            bool aux = buttonIsClicked;
            buttonIsClicked = false;
            return aux;
        }

        /// <summary>
        /// Função chamada para ativar a decisão.
        /// </summary>
        public void Click()
        {
            buttonIsClicked = true;        
        }
    }

}
