using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu
{
    /// <summary>
    /// Classe responsável por gerenciar as informações do personagem no menu de skills.
    /// </summary>
    public class HeroInfo : MonoBehaviour
    {
        /// <summary>
        /// Referência para o heroi que está em seu turno.
        /// </summary>
        private Hero currentHero;

        [Header("UI elements")]
        /// <summary>
        /// Campo de texto onde ficará o nome do personagem.
        /// </summary>
        [SerializeField] private Text nameText;

        /// <summary>
        /// Campo de imagem onde ficará o portrait do personagem.
        /// </summary>
        [SerializeField] private Image heroImage;

        /// <summary>
        /// Slider que mostrará a vida atual e máxima do personagem.
        /// </summary>
        [SerializeField] private Slider hpSlider;

        /// <summary>
        /// Campo de texto que mostrará a vida atual e máxima do personagem.
        /// </summary>
        [SerializeField] private Text hpText;

        /// <summary>
        /// Campo de texto onde ficará o dano do personagem.
        /// </summary>
        [SerializeField] private Text damageText;

        /// <summary>
        /// Campo de texto onde ficará a defesa do personagem.
        /// </summary>
        [SerializeField] private Text resistanceText;

        /// <summary>
        /// Campo de texto onde ficará a defesa mágica do personagem.
        /// </summary>
        [SerializeField] private Text magicResistanceText;

        /// <summary>
        /// Campo de texto onde ficará a velocidade do personagem.
        /// </summary>
        [SerializeField] private Text speedText;

        /// <summary>
        /// Carrega as informações do personagem no menu.
        /// </summary>
        /// <param name="currentUnit"> Referência ao heroi que está em seu turno. </param>
        public void LoadInfo(BattleUnit currentUnit)
        {
            currentHero = (Hero) currentUnit.unit;

            nameText.text = currentHero.name.ToUpper();
            nameText.color = currentHero.color;

            heroImage.sprite = currentHero.portrait;

            hpSlider.maxValue = currentHero.hpMax;
            hpSlider.value = currentUnit.curHP;
            hpText.text = currentUnit.curHP + "/" + currentHero.hpMax;

            damageText.text = currentUnit.curDmg.ToString();
            resistanceText.text = currentUnit.curDef.ToString();
            magicResistanceText.text = currentUnit.curMagicDef.ToString();
            speedText.text = currentUnit.curSpeed.ToString();
        }

        /// <summary>
        /// Retorna a lista de skills do heroi que está em seu turno.
        /// </summary>
        public List<Skill> GetCurrentHeroSkills()
        {
            return currentHero.skills;
        }
    }

}