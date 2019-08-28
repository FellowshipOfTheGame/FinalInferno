using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Pause State")]
    public class ChangePauseStateAction : Action
    {
        public override void Act(StateController controller)
        {
            PauseMenu.ChangePauseState();
        }

    }
}
