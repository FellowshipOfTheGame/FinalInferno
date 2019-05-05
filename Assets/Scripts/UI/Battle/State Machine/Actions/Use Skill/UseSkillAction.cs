using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.Battle;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Use Skill")]
    public class UseSkillAction : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            BattleSkillManager.UseSkill();
        }

    }

}