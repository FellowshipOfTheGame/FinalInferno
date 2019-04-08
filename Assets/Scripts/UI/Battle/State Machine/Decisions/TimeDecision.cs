using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleUI SM/Decisions/Time")]
public class ScanDecision : Decision
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
