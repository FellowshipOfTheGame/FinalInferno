using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI
{
    public class HeroInfo : MonoBehaviour
    {
        private Hero currentHero;

        public SkillList skillList;

        public Text nameText;
        public Image heroImage;
        public Slider hpSlider;
        public Text damageText;
        public Text resistanceText;
        public Text magicResistanceText;
        public Text speedText;

        public void LoadInfo(BattleUnit currentUnit)
        {
            currentHero = (Hero) currentUnit.unit;

            nameText.text = currentHero.name.ToUpper();
            nameText.color = currentHero.color;

            heroImage.sprite = currentHero.portrait;

            hpSlider.maxValue = currentHero.hpMax;
            hpSlider.value = currentUnit.curHP;

            damageText.text = currentUnit.curDmg.ToString();
            resistanceText.text = currentUnit.curDef.ToString();
            magicResistanceText.text = currentUnit.curMagicDef.ToString();
            speedText.text = currentUnit.curSpeed.ToString();
        }

        public List<Skill> GetCurrentHeroSkills()
        {
            return currentHero.skills;
        }
    }

}