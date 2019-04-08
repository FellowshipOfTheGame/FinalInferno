using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleUI SM/State")]
public class State : ScriptableObject
{
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        CheckTransitions(controller);
    }

    private void CheckTransitions(StateController controller)
    {
        foreach (Transition T in transitions)
        {
            bool decisionSucceeded = false;
            foreach (Decision D in T.decisions)
            {
                decisionSucceeded = decisionSucceeded && D.Decide(controller);
            }

            if (decisionSucceeded)
            {
                controller.TransitionToState(T.nextState, T.actions);
            }
        }
    }


}
