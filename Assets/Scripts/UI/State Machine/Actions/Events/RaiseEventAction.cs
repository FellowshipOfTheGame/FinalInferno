using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Raise Event")]
    public class RaiseEventAction : Action
    {
        [SerializeField] private FinalInferno.EventSystem.EventFI eventToRaise;
        public override void Act(StateController controller)
        {
            if(eventToRaise != null){
                eventToRaise.Raise();
            }
        }
    }
}