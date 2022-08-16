﻿using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Raise Event")]
    public class RaiseEventAction : Action {
        [SerializeField] private EventFI eventToRaise;
        public override void Act(StateController controller) {
            if (eventToRaise)
                eventToRaise.Raise();
        }
    }
}