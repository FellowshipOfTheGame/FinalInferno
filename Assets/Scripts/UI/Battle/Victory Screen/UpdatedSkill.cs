using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Victory
{
    public class UpdatedSkill : MonoBehaviour
    {
        [SerializeField] private Text SkillName;
        [SerializeField] private Image SkillImage;
        [SerializeField] private Text SkillLevelText;
        [SerializeField] private Slider SkillLevelSlider;
        [SerializeField] private Image PreviousXPAmountImage;
        [SerializeField] private Text UpdateText;

        [SerializeField] private float _duration = 1f, levelUpTime = .8f;

        public PlayerSkill thisSkill { get; private set; }
        private float _time;
        private long xpGained;

        public void LoadUpdatedSkill(SkillInfo pSkill, PlayerSkill skill)
        {
            SkillName.text = skill.name;
            SkillImage.sprite = skill.skillImage;
            SkillLevelText.text = pSkill.level.ToString();
            SkillLevelSlider.maxValue = pSkill.xpToNextLevel;
            SkillLevelSlider.value = pSkill.xp;
            PreviousXPAmountImage.fillAmount = pSkill.xp / (float) pSkill.xpToNextLevel;

            xpGained = skill.XpCumulative - pSkill.xpCumulative;
            if(xpGained > (pSkill.xpToNextLevel - pSkill.xp)){
                xpGained = (pSkill.xpToNextLevel - pSkill.xp) + skill.xp;
            }
            thisSkill = skill;
        }

        public void LoadNewSkill(PlayerSkill skill)
        {
            SkillName.text = skill.name;
            SkillLevelText.text = skill.Level.ToString();
            SkillLevelSlider.maxValue = skill.xpNext;
            SkillLevelSlider.value = skill.xp;
            PreviousXPAmountImage.fillAmount = skill.xp / (float) skill.xpNext;

            UpdateText.text = "NEW SKILL!";
            UpdateText.gameObject.SetActive(true);

            xpGained = 0;
            thisSkill = skill;
        }

        public void StartAnimation()
        {
            _time = 0f;
            StartCoroutine(SkillLevelAnimation());
        }

        private IEnumerator SkillLevelAnimation()
        {
            while (_time <= _duration)
            {
                SkillLevelSlider.value += Time.fixedDeltaTime * xpGained / _duration;
                if (SkillLevelSlider.value >= SkillLevelSlider.maxValue)
                {
                    LevelUp();
                    //yield return new WaitForSeconds(levelUpTime);
                }
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            SkillLevelSlider.value = thisSkill.xp;
        }

        private void LevelUp()
        {
            SkillLevelText.text = thisSkill.Level.ToString();
            PreviousXPAmountImage.fillAmount = 0f;
            SkillLevelSlider.value = 0f;
            SkillLevelSlider.maxValue = thisSkill.xpNext;
            UpdateText.gameObject.SetActive(true);
        }
    }
}