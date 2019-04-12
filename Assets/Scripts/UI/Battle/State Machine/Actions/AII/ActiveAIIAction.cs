using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Active AII")]
    public class ActiveAIIAction : ComponentRequester
    {
        private AIIManager manager;
        public override void Act(StateController controller)
        {
            manager.Active();
        }

        public override void RequestComponent(GameObject provider)
        {
            RequestAIIManager(provider);
        }

        private void RequestAIIManager(GameObject provider)
        {
            manager = provider.GetComponent<AIIManager>();
        }

    }

}