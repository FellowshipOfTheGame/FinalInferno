using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que salva o jogo.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Save Game")]
    public class SaveGame : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller)
        {
            SaveLoader.SaveGame();
        }

    }

}