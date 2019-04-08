using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Button State")]
public class ChangeButtonStateAction : Action
{
    [SerializeField] private Button button;
    public override void Act(StateController controller)
    {
        ChangeButtonState(controller);
    }

    public void SetButton(Button newButton)
    {
        button = newButton;
    }

    private void ChangeButtonState(StateController controller)
    {
        button.interactable = !button.interactable;
    }
}
