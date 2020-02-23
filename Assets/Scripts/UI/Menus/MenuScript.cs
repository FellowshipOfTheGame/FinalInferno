using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{
    public class MenuScript : MonoBehaviour
    {
        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        public void SceneLoadCallback(){
            if(SceneLoader.onSceneLoad != null )
                SceneLoader.onSceneLoad();
        }

        public void NewGame()
        {
            SaveLoader.NewGame();
        }

        public void LoadGame()
        {
            SaveLoader.LoadGame();
        }
    }
}