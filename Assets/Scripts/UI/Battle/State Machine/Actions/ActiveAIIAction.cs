using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Active AII")]
    public class ActiveAIIAction : Action
    {
        [SerializeField] private AIIManager manager;
        public override void Act(StateController controller)
        {
            manager.Active();
        }

        public void SetAII(AIIManager newManager)
        {
            manager = newManager;
        }
    }

}