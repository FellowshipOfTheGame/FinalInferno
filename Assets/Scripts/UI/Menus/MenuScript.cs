using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI
{
    public class MenuScript : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
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