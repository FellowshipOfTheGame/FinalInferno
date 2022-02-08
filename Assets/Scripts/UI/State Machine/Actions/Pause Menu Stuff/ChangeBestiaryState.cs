using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que fecha ou abre o bestiário.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Bestiary State")]
    public class ChangeBestiaryState : ComponentRequester {
        /// <summary>
        /// Referência ao bestiário.
        /// </summary>
        private BestiaryMenu bestiary;

        /// <summary>
        /// Parâmetro bool que determina se deve abrir ou fecharo bestiário.
        /// </summary>
        [SerializeField] private bool shouldOpen;

        /// <summary>
        /// Executa uma ação.
        /// Abre ou fecha o bestiário.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            if (shouldOpen) {
                bestiary.OpenBestiary();
            } else {
                bestiary.CloseBestiary();
            }
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Requisita um animator.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider) {
            bestiary = provider.GetComponent<BestiaryMenu>();
        }

    }

}