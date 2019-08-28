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
        /// Campo de texto onde ficará o dano do personagem.
        /// </summary>
        [SerializeField] private Text alteredDamageText;

        /// <summary>
        /// Campo de texto onde ficará a defesa do personagem.
        /// </summary>
        [SerializeField] private Text alteredResistanceText;

        /// <summary>
        /// Campo de texto onde ficará a defesa mágica do personagem.
        /// </summary>
        [SerializeField] private Text alteredMagicResistanceText;

        /// <summary>
        /// Campo de texto onde ficará a velocidade do personagem.
        /// </summary>
        [SerializeField] private Text alteredSpeedText;

        [SerializeField] private List<Skill> activeSkills;

        [Header("Colors")]
        [SerializeField] private Color positiveColor;
        [SerializeField] private Color negativeColor;

        [SerializeField] private FinalInferno.UI.AII.AIIManager AIISkillsList;

        /// <summary>
        /// Carrega as informações do personagem no menu.
        /// </summary>
        /// <param name="currentUnit"> Referência ao heroi que está em seu turno. </param>
        public void LoadInfo(BattleUnit currentUnit)
        {
            currentHero = (Hero) currentUnit.unit;
            activeSkills = new List<Skill>(currentUnit.ActiveSkills);

            nameText.text = currentHero.name.ToUpper();
            nameText.color = currentHero.color;

            heroImage.sprite = currentUnit.Portrait;

            hpSlider.maxValue = currentHero.hpMax;
            hpSlider.value = currentUnit.CurHP;
            hpText.text = currentUnit.CurHP + "/" + currentUnit.MaxHP;

            damageText.text = currentUnit.unit.baseDmg.ToString();
            if (currentUnit.curDmg > currentUnit.unit.baseDmg)
            {
                alteredDamageText.text = "(+" + (currentUnit.curDmg - currentUnit.unit.baseDmg).ToString() + ")";
                alteredDamageText.color = positiveColor;
            }
            else if (currentUnit.curDmg < currentUnit.unit.baseDmg)
            {
                alteredDamageText.text = "(" + (currentUnit.curDmg - currentUnit.unit.baseDmg).ToString() + ")";
                alteredDamageText.color = negativeColor;
            }
            else
                alteredDamageText.text = "";
                
            resistanceText.text = currentUnit.unit.baseDef.ToString();
            if (currentUnit.curDef > currentUnit.unit.baseDef)
            {
                alteredResistanceText.text = "(+" +(currentUnit.curDef - currentUnit.unit.baseDef).ToString() + ")";
                alteredResistanceText.color = positiveColor;
            }
            else if (currentUnit.curDef < currentUnit.unit.baseDef)
            {
                alteredResistanceText.text = "(" + (currentUnit.curDef - currentUnit.unit.baseDef).ToString() + ")";
                alteredResistanceText.color = negativeColor;
            }
            else
                alteredResistanceText.text = "";

            magicResistanceText.text = currentUnit.unit.baseMagicDef.ToString();
            if (currentUnit.curMagicDef > currentUnit.unit.baseMagicDef)
            {
                alteredMagicResistanceText.text = "(+" + (currentUnit.curMagicDef - currentUnit.unit.baseMagicDef).ToString() + ")";
                alteredMagicResistanceText.color = positiveColor;
            }
            else if (currentUnit.curMagicDef < currentUnit.unit.baseMagicDef)
            {
                alteredMagicResistanceText.text = "(" + (currentUnit.curMagicDef - currentUnit.unit.baseMagicDef).ToString() + ")";
                alteredMagicResistanceText.color = negativeColor;
            }
            else
                alteredMagicResistanceText.text = "";

            speedText.text = currentUnit.unit.baseSpeed.ToString();
            if (currentUnit.curSpeed > currentUnit.unit.baseSpeed)
            {
                alteredSpeedText.text = "(+" +(currentUnit.curSpeed - currentUnit.unit.baseSpeed).ToString() + ")";
                alteredSpeedText.color = positiveColor;
            }
            else if (currentUnit.curSpeed < currentUnit.unit.baseSpeed)
            {
                alteredSpeedText.text = "(" + (currentUnit.curSpeed - currentUnit.unit.baseSpeed).ToString() + ")";
                alteredSpeedText.color = negativeColor;
            }
            else
                alteredSpeedText.text = "";

        }

        /// <summary>
        /// Retorna a lista de skills do heroi que está em seu turno.
        /// </summary>
        public List<Skill> GetCurrentHeroSkills()
        {
            return activeSkills;
        }

        public void ChangeAIISkillsList()
        {
            AIISkillsList.Active();
        }
    }

}