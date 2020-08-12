using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle.QueueMenu;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Stop Queue Preview")]
    public class HideQueuePreview : ComponentRequester
    {
        [SerializeField] private BattleQueueUI queue;

        public override void Act(StateController controller){
            if(queue != null)
                queue.StopPreview();
        }

        public override void RequestComponent(GameObject provider){
            queue = provider.GetComponent<BattleQueueUI>();
        }
    }
}