using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            SkillLevelText.text = pSkill.level.ToString();
            SkillLevelSlider.maxValue = skill.xpNext;
            SkillLevelSlider.value = pSkill.xp;
            PreviousXPAmountImage.fillAmount = pSkill.xp / (float) skill.xpNext;

            xpGained = skill.XpCumulative - pSkill.xpCumulative;
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
                    yield return new WaitForSeconds(levelUpTime);
                }
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            SkillLevelSlider.value = thisSkill.xp;
        }

        private void LevelUp()
        {
            SkillLevelText.text = (int.Parse(SkillLevelText.text) + 1).ToString();
            PreviousXPAmountImage.fillAmount = 0f;
            SkillLevelSlider.value = 0f;
            SkillLevelSlider.maxValue = thisSkill.xpNext;
            UpdateText.gameObject.SetActive(true);
        }
    }
}