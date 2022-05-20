using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que carrega as informações do personagem.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Info")]
    public class LoadInfoAction : ComponentRequester {
        /// <summary>
        /// Gerenciador das informações do personagem.
        /// </summary>
        private HeroInfo info;

        /// <summary>
        /// Executa uma ação.
        /// Carrega as informações do personagem.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller) {
            info.LoadInfo(BattleManager.instance.CurrentUnit);
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede os componentes HeroInfo ao respectivo responsável.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider) {
            HeroInfo newInfo = provider.GetComponent<HeroInfo>();
            if (newInfo != null) {
                info = newInfo;
            }
        }
    }

}