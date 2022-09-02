using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Update Lives")]
    public class UpdateLives : Action {
        [SerializeField] private EventFI updateLivesEvent;
        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            updateLivesEvent.Raise();
        }

    }

}