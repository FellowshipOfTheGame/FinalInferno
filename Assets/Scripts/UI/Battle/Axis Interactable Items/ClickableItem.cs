using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    public class ClickableItem : AxisInteractableItem
    {
        [SerializeField] private ButtonClickDecision BCD;

        void Start()
        {
            OnAct += Click;
        }

        private void Click()
        {
            BCD.Click();
        }
    }

}
