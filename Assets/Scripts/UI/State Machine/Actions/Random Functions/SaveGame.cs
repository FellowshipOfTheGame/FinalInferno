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
            // Armazena a posicao dos personagens no overworld dentro do SO correspondente
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                if(Party.Instance.characters[i].OverworldInstance != null)
                    Party.Instance.characters[i].position = Party.Instance.characters[i].OverworldInstance.transform.position;
            }
            SaveLoader.SaveGame();
        }

    }

}