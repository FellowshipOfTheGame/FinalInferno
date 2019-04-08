using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleUI SM/Actions/Set Trigger")]
public class SetTriggerAction : Action
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger;
    public override void Act(StateController controller)
    {
        SetTrigger(controller);
    }

    private void SetTrigger(StateController controller)
    {
        animator.SetTrigger(trigger);
    }
}
