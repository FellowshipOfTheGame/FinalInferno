﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que inicia a batalha.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Start Battle")]
    public class StartBattle : Action
    {
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller)
        {
            BattleManager.instance.StartBattle();
        }

    }

}