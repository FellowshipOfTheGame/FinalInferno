using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que carrega a lista de skills.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Skill List")]
    public class LoadSkillListAction : ComponentRequester
    {
        /// <summary>
        /// Gerenciador das informações do personagem.
        /// </summary>
        private HeroInfo info;

        /// <summary>
        /// Lista de skills.
        /// </summary>
        private SkillList skillList;

        /// <summary>
        /// Executa uma ação.
        /// Atualiza a lista de skills.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            skillList.UpdateSkillsContent(info.GetCurrentHeroSkills());
        }

        /// <summary>
        /// Função chamada para pedir um componente ao provedor.
        /// Pede os componentes HeroInfo e SkillList aos respectivos responsáveis.
        /// </summary>
        /// <param name="provider"> Game object que provê o componente desejado. </param>
        public override void RequestComponent(GameObject provider)
        {
            HeroInfo newInfo = provider.GetComponent<HeroInfo>();
            SkillList newSkillList = provider.GetComponent<SkillList>();
            if (newInfo != null)
                info = newInfo;
            if (newSkillList != null)
                skillList = newSkillList;
        }
    }

}