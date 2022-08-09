using FinalInferno.UI.FSM;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class ClickableItem : MonoBehaviour {
        public ButtonClickDecision buttonClickDecision;

        [SerializeField] private AxisInteractableItem item;

        private void Awake() {
            item.OnAct += Click;
        }

        private void Click() {
            buttonClickDecision.Click();
        }
    }

}
