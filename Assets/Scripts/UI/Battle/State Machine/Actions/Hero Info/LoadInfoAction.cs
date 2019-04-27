using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que carrega as informações do personagem.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Info")]
    public class LoadInfoAction : ComponentRequester
    {
        /// <summary>
        /// Gerenciador das informações do personagem.
        /// </summary>
        private HeroInfo info;

        /// <summary>
        /// Gerenciador do sistema de batalhas.
        /// </summary>
        private BattleDebug battle;

        /// <summary>
        /// Executa uma ação.
        /// Carrega as informações do personagem.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            info.LoadInfo(battle.GetCurrentUnit());
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede os componentes HeroInfo e Battle aos respectivos responsáveis.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            HeroInfo newInfo = provider.GetComponent<HeroInfo>();
            BattleDebug newBattle = provider.GetComponent<BattleDebug>();
            if (newInfo != null)
                info = newInfo;
            if (newBattle != null)
                battle = newBattle;
        }
    }

}