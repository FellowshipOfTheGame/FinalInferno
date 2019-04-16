using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.FSM;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// A type of item that can be clicked.
	/// </summary>
    public class ClickableItem : AxisInteractableItem
    {
        /// <summary>
        /// Reference to the button click decision SO.
        /// </summary>
        [SerializeField] private ButtonClickDecision BCD;

        void Start()
        {
            OnAct += Click;
        }

        /// <summary>
        /// Calls the button click decision trigger.
        /// </summary>
        private void Click()
        {
            BCD.Click();
        }
    }

}
