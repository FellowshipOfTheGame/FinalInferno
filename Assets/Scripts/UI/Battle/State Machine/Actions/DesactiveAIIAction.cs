using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Desactive AII")]
    public class DesactiveAIIAction : Action
    {
        [SerializeField] private AIIManager manager;
        public override void Act(StateController controller)
        {
            manager.Desactive();
        }

        public void SetAII(AIIManager newManager)
        {
            manager = newManager;
        }
    }

}