﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Time")]
    public class TimeDecision : Decision
    {
        public float stateTime;
        public override bool Decide(StateController controller)
        {
            return CheckStateTime(controller);
        }

        private bool CheckStateTime(StateController controller)
        {
            return controller.CheckIfCountDownElapsed(stateTime);
        }
    }

}
