using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{

    public class ChangeSceneUI : MonoBehaviour
    {
        private void MainMenu()
        {
            SceneLoader.LoadMainMenu();
        }

        private void ReturnCheckpoint()
        {
            // TO DO: Carrega o jogo salvo no slot atual
            //SceneLoader.LoadOWScene(SceneLoader.LastOWScene, true);
        }

        private void Continue()
        {
            SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID, true);
        }
    }

}