using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{

    public class ChangeSceneUI : MonoBehaviour
    {
        private void MainMenu()
        {
            //SceneLoader.LoadMainMenu();
            StaticLoadStuff.gameStarted = false;
            SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID);
        }

        private void ReturnCheckpoint()
        {
            // TO DO: Carrega o jogo salvo no slot atual
            //SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID, true);
            StaticLoadStuff.gameStarted = false;
            SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID);
        }

        private void Continue()
        {
            SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID, true);
        }
    }

}