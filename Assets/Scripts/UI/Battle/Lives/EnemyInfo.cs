﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu
{
    /// <summary>
    /// Classe responsável por gerenciar as informações do personagem no menu de inimigo.
    /// </summary>
    public class EnemyInfo : UnitLife
    {
        /// <summary>
        /// Referência para o inimigo que está sendo mostrado.
        /// </summary>
        private Enemy thisEnemy;

        [Header("UI elements")]
        /// <summary>
        /// Campo de texto onde ficará o nome do personagem.
        /// </summary>
        [SerializeField] private Text nameText;

        /// <summary>
        /// Campo de imagem onde ficará o portrait do personagem.
        /// </summary>
        [SerializeField] private Image enemyImage;

        /// <summary>
        /// Slider que mostrará a vida atual e máxima do personagem.
        /// </summary>
        [SerializeField] private Slider hpSlider;

        /// <summary>
        /// Campo de texto que mostrará a vida atual e máxima do personagem.
        /// </summary>
        [SerializeField] private Text hpText;

        /// <summary>
        /// Carrega as informações do personagem no menu.
        /// </summary>
        public override void UpdateUnitLife()
        {
            thisEnemy = (Enemy) thisUnit.unit;

            nameText.text = thisEnemy.name.ToUpper();
            nameText.color = thisEnemy.color;

            enemyImage.sprite = thisUnit.Portrait;

            hpSlider.maxValue = thisEnemy.hpMax;
            hpSlider.value = thisUnit.CurHP;
            hpText.text = thisUnit.CurHP + "/" + thisUnit.MaxHP;
        }
    }

}