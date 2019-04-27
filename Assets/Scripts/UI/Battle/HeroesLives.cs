using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle.LifeMenu
{
    /// <summary>
    /// Classe responsável por gerenciar o menu de vidas.
    /// </summary>
    public class HeroesLives : MonoBehaviour
    {
        public delegate void HeroUpdate();
        public event HeroUpdate OnUpdate;

        public void UpdateLives()
        {
            OnUpdate();
        }

    }

}