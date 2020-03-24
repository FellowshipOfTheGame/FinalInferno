using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que inicia a batalha.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Scene Change Callback")]
    public class SceneChangeCallback : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller)
        {
            SceneLoader.onSceneLoad?.Invoke();
        }

    }

}