using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu
{

    /// <summary>
    /// Classe responsável por mostrar a vida máxima e atual de um heroi.
    /// </summary>
    public class HeroLife : MonoBehaviour
    {
        /// <summary>
        /// Referência para o gerenciador do menu de vidas.
        /// </summary>
        [SerializeField] private HeroesLives manager;

        /// <summary>
        /// Heroi que será mostrado as informações de vida.
        /// </summary>
        private BattleUnit thisHero;

        /// <summary>
        /// Campo de texto onde serão mostradas as informações de vida do heroi.
        /// </summary>
        [SerializeField] private Text heroText;

        /// <summary>
        /// Adiciona a atualização da vida desse heroi no evento do gerenciador.
        /// </summary>
        void Awake()
        {
            manager.OnUpdate += UpdateHeroLife;
        }

        /// <summary>
        /// Atualiza o campo de texto com as informações de vida do heroi e também com sua cor.
        /// </summary>
        public void UpdateHeroLife()
        {
            heroText.text = thisHero.unit.name + " - " + thisHero.curHP + "/" + thisHero.unit.hpMax;
            heroText.color = thisHero.unit.color;
        }
    }

}