using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Decision based in whether a button is clicked or not.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Button Click")]
    public class ButtonClickDecision : Decision
    {
        /// <summary>
        /// Variable that check the state of the button.
        /// </summary>
        [SerializeField] private bool buttonIsClicked;

        /// <summary>
        /// Verify if the decision triggers.
        /// </summary>
        /// <param name="controller"> The Finite State Machine controller. </param>
        public override bool Decide(StateController controller)
        {
            bool aux = buttonIsClicked;
            buttonIsClicked = false;
            return aux;
        }

        /// <summary>
        /// Function called on a button click event.
        /// </summary>
        public void Click()
        {
            buttonIsClicked = true;        
        }
    }

}
