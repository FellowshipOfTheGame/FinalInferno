using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Decisão baseada na capacidade dos personagens de se moverem no overworld.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Save Game")]
    public class CanSaveGame : Decision {
        /// <summary>
        /// A situação desejada para a unidade.
        /// </summary>
        [SerializeField] private bool canSave;

        /// <summary>
        /// Verifica se a unidade está na situação desejada.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override bool Decide(StateController controller) {
            return (canSave == SaveLoader.CanSaveGame);
        }

    }

}