using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Desactive AII")]
    public class DesactiveAIIAction : ComponentRequester
    {
        private AIIManager manager;
        public override void Act(StateController controller)
        {
            manager.Desactive();
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