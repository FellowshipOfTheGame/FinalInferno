using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class HeroInfo : MonoBehaviour {
        [Header("UI elements")]
        [SerializeField] private Text nameText;
        [SerializeField] private Image heroImage;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Text hpText;
        [SerializeField] private Text damageText;
        [SerializeField] private Text resistanceText;
        [SerializeField] private Text magicResistanceText;
        [SerializeField] private Text speedText;
        [SerializeField] private Text alteredDamageText;
        [SerializeField] private Text alteredResistanceText;
        [SerializeField] private Text alteredMagicResistanceText;
        [SerializeField] private Text alteredSpeedText;
        [SerializeField] private List<Skill> activeSkills;
        [Header("Colors")]
        [SerializeField] private Color positiveColor;
        [SerializeField] private Color negativeColor;
        [SerializeField] private SkillList skillList;
        [SerializeField] private AII.AIIManager buttonAIIManager;
        private Hero currentHero;

        public void LoadInfo(BattleUnit currentUnit) {
            currentHero = (Hero)currentUnit.Unit;
            activeSkills = new List<Skill>(currentUnit.ActiveSkills);
            ShowHeroGeneralInfo(currentUnit);
            ShowHeroStats(currentUnit);
        }

        private void ShowHeroGeneralInfo(BattleUnit currentUnit) {
            nameText.text = currentHero.name.ToUpper();
            nameText.color = currentHero.color;
            heroImage.sprite = currentUnit.Portrait;
        }

        private void ShowHeroStats(BattleUnit currentUnit) {
            ShowHpValues(currentUnit);
            damageText.text = currentUnit.Unit.baseDmg.ToString();
            ShowAlteredDamage(currentUnit);
            resistanceText.text = currentUnit.Unit.baseDef.ToString();
            ShowAlteredDefense(currentUnit);
            magicResistanceText.text = currentUnit.Unit.baseMagicDef.ToString();
            ShowAlteredMagicResistance(currentUnit);
            speedText.text = currentUnit.Unit.baseSpeed.ToString();
            ShowAlteredSpeed(currentUnit);
        }

        private void ShowHpValues(BattleUnit currentUnit) {
            hpSlider.maxValue = currentHero.hpMax;
            hpSlider.value = currentUnit.CurHP;
            hpText.text = $"{currentUnit.CurHP}/{currentUnit.MaxHP}";
        }

        private void ShowAlteredDamage(BattleUnit currentUnit) {
            if (currentUnit.CurDmg > currentUnit.Unit.baseDmg) {
                alteredDamageText.text = $"+{currentUnit.CurDmg - currentUnit.Unit.baseDmg}";
                alteredDamageText.color = positiveColor;
            } else if (currentUnit.CurDmg < currentUnit.Unit.baseDmg) {
                alteredDamageText.text = $"{currentUnit.CurDmg - currentUnit.Unit.baseDmg}";
                alteredDamageText.color = negativeColor;
            } else {
                alteredDamageText.text = "";
            }
        }

        private void ShowAlteredDefense(BattleUnit currentUnit) {
            if (currentUnit.CurDef > currentUnit.Unit.baseDef) {
                alteredResistanceText.text = $"+{currentUnit.CurDef - currentUnit.Unit.baseDef}";
                alteredResistanceText.color = positiveColor;
            } else if (currentUnit.CurDef < currentUnit.Unit.baseDef) {
                alteredResistanceText.text = $"{currentUnit.CurDef - currentUnit.Unit.baseDef}";
                alteredResistanceText.color = negativeColor;
            } else {
                alteredResistanceText.text = "";
            }
        }

        private void ShowAlteredMagicResistance(BattleUnit currentUnit) {
            if (currentUnit.CurMagicDef > currentUnit.Unit.baseMagicDef) {
                alteredMagicResistanceText.text = $"+{currentUnit.CurMagicDef - currentUnit.Unit.baseMagicDef}";
                alteredMagicResistanceText.color = positiveColor;
            } else if (currentUnit.CurMagicDef < currentUnit.Unit.baseMagicDef) {
                alteredMagicResistanceText.text = $"{currentUnit.CurMagicDef - currentUnit.Unit.baseMagicDef}";
                alteredMagicResistanceText.color = negativeColor;
            } else {
                alteredMagicResistanceText.text = "";
            }
        }

        private void ShowAlteredSpeed(BattleUnit currentUnit) {
            if (currentUnit.CurSpeed > currentUnit.Unit.baseSpeed) {
                alteredSpeedText.text = $"+{currentUnit.CurSpeed - currentUnit.Unit.baseSpeed}";
                alteredSpeedText.color = positiveColor;
            } else if (currentUnit.CurSpeed < currentUnit.Unit.baseSpeed) {
                alteredSpeedText.text = $"{currentUnit.CurSpeed - currentUnit.Unit.baseSpeed}";
                alteredSpeedText.color = negativeColor;
            } else {
                alteredSpeedText.text = "";
            }
        }

        public List<Skill> GetCurrentHeroSkills() {
            return activeSkills;
        }

        public void ChangeAIISkillsList() {
            skillList.ActivateManager();
        }

        public void ChangeAIIButtons() {
            buttonAIIManager.Activate();
        }
    }

}