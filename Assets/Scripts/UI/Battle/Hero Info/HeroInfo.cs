using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu {
    /// <summary>
    /// Classe responsável por gerenciar as informações do personagem no menu de skills.
    /// </summary>
    public class HeroInfo : MonoBehaviour {
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

        [SerializeField] private FinalInferno.UI.Battle.SkillMenu.SkillList skillList;
        [SerializeField] private FinalInferno.UI.AII.AIIManager buttonAIIManager;

        /// <summary>
        /// Carrega as informações do personagem no menu.
        /// </summary>
        /// <param name="currentUnit"> Referência ao heroi que está em seu turno. </param>
        public void LoadInfo(BattleUnit currentUnit) {
            currentHero = (Hero)currentUnit.Unit;
            activeSkills = new List<Skill>(currentUnit.ActiveSkills);

            nameText.text = currentHero.name.ToUpper();
            nameText.color = currentHero.color;

            heroImage.sprite = currentUnit.Portrait;

            hpSlider.maxValue = currentHero.hpMax;
            hpSlider.value = currentUnit.CurHP;
            hpText.text = currentUnit.CurHP + "/" + currentUnit.MaxHP;

            damageText.text = currentUnit.Unit.baseDmg.ToString();
            if (currentUnit.CurDmg > currentUnit.Unit.baseDmg) {
                alteredDamageText.text = "+" + (currentUnit.CurDmg - currentUnit.Unit.baseDmg).ToString();
                alteredDamageText.color = positiveColor;
            } else if (currentUnit.CurDmg < currentUnit.Unit.baseDmg) {
                alteredDamageText.text = "" + (currentUnit.CurDmg - currentUnit.Unit.baseDmg).ToString();
                alteredDamageText.color = negativeColor;
            } else {
                alteredDamageText.text = "";
            }

            resistanceText.text = currentUnit.Unit.baseDef.ToString();
            if (currentUnit.CurDef > currentUnit.Unit.baseDef) {
                alteredResistanceText.text = "+" + (currentUnit.CurDef - currentUnit.Unit.baseDef).ToString();
                alteredResistanceText.color = positiveColor;
            } else if (currentUnit.CurDef < currentUnit.Unit.baseDef) {
                alteredResistanceText.text = "" + (currentUnit.CurDef - currentUnit.Unit.baseDef).ToString();
                alteredResistanceText.color = negativeColor;
            } else {
                alteredResistanceText.text = "";
            }

            magicResistanceText.text = currentUnit.Unit.baseMagicDef.ToString();
            if (currentUnit.CurMagicDef > currentUnit.Unit.baseMagicDef) {
                alteredMagicResistanceText.text = "+" + (currentUnit.CurMagicDef - currentUnit.Unit.baseMagicDef).ToString();
                alteredMagicResistanceText.color = positiveColor;
            } else if (currentUnit.CurMagicDef < currentUnit.Unit.baseMagicDef) {
                alteredMagicResistanceText.text = "" + (currentUnit.CurMagicDef - currentUnit.Unit.baseMagicDef).ToString();
                alteredMagicResistanceText.color = negativeColor;
            } else {
                alteredMagicResistanceText.text = "";
            }

            speedText.text = currentUnit.Unit.baseSpeed.ToString();
            if (currentUnit.CurSpeed > currentUnit.Unit.baseSpeed) {
                alteredSpeedText.text = "+" + (currentUnit.CurSpeed - currentUnit.Unit.baseSpeed).ToString();
                alteredSpeedText.color = positiveColor;
            } else if (currentUnit.CurSpeed < currentUnit.Unit.baseSpeed) {
                alteredSpeedText.text = "" + (currentUnit.CurSpeed - currentUnit.Unit.baseSpeed).ToString();
                alteredSpeedText.color = negativeColor;
            } else {
                alteredSpeedText.text = "";
            }
        }

        /// <summary>
        /// Retorna a lista de skills do heroi que está em seu turno.
        /// </summary>
        public List<Skill> GetCurrentHeroSkills() {
            return activeSkills;
        }

        public void ChangeAIISkillsList() {
            skillList.ActivateManager();
        }

        public void ChangeAIIButtons() {
            buttonAIIManager.Active();
        }
    }

}