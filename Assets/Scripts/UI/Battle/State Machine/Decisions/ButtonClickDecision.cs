using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Button Click")]
    public class ButtonClickDecision : Decision
    {
        [SerializeField] private bool buttonIsClicked;

        public override bool Decide(StateController controller)
        {
            bool aux = buttonIsClicked;
            buttonIsClicked = false;
            return aux;
        }

        public void Click()
        {
            buttonIsClicked = true;        
        }
    }

}
