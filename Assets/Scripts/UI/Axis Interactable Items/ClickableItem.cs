using FinalInferno.UI.FSM;
using UnityEngine;

namespace FinalInferno.UI.AII {
    /// <summary>
	/// Tipo de item que pode ser clicado.
	/// </summary>
    public class ClickableItem : MonoBehaviour {
        /// <summary>
        /// Referência ao decisor de clique.
        /// </summary>
        public ButtonClickDecision BCD;

        /// <summary>
        /// Referência ao item da lista.
        /// </summary>
        [SerializeField] private AxisInteractableItem item;

        private void Awake() {
            item.OnAct += Click;
        }

        /// <summary>
        /// Ativa o clique do botão.
        /// </summary>
        private void Click() {
            // Debug.Log("ATENSAO");
            BCD.Click();
            // Debug.Log("ATENSAAAO");

        }
    }

}
