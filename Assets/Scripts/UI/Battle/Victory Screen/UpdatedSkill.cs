using System.Collections;
using FinalInferno.UI.AII;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory {
    public class UpdatedSkill : MonoBehaviour {
        private const string newSkillText = "NEW SKILL!";
        private const string leveledSkillText = "NEW LEVEL!";
        [SerializeField] private Text SkillName;
        [SerializeField] private Image skillImage;
        [SerializeField] private Text SkillLevelText;
        [SerializeField] private Slider SkillLevelSlider;
        [SerializeField] private Image PreviousXPAmountImage;
        [SerializeField] private Text UpdateText;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private VictorySkillListItem victorySkillListItem;
        public VictorySkillListItem VictorySkillListItem => victorySkillListItem;
        public PlayerSkill ThisSkill { get; private set; }
        private float _time;
        private long xpGained;

        public void LoadUpdatedSkill(SkillInfo skillPreviousInfo, PlayerSkill skill) {
            SkillName.text = skill.name;
            skillImage.sprite = skill.skillImage;
            SkillLevelText.text = $"{skillPreviousInfo.level}";
            SkillLevelSlider.maxValue = skillPreviousInfo.xpToNextLevel;
            SkillLevelSlider.value = skillPreviousInfo.xp;
            PreviousXPAmountImage.fillAmount = skillPreviousInfo.xp / (float)skillPreviousInfo.xpToNextLevel;
            UpdateText.text = string.Empty;
            UpdateText.gameObject.SetActive(false);
            xpGained = CalculateShownXPGain(skillPreviousInfo, skill);
            ThisSkill = skill;
        }

        private long CalculateShownXPGain(SkillInfo skillPreviousInfo, PlayerSkill skill) {
            long gain = skill.XpCumulative - skillPreviousInfo.xpCumulative;
            long xpToLevelUp = skillPreviousInfo.xpToNextLevel - skillPreviousInfo.xp;
            return gain > xpToLevelUp ? xpToLevelUp + skill.xp : gain;
        }

        public void LoadNewSkill(PlayerSkill skill) {
            SkillName.text = skill.name;
            skillImage.sprite = skill.skillImage;
            SkillLevelText.text = $"{skill.Level}";
            SkillLevelSlider.maxValue = skill.xpNext;
            SkillLevelSlider.value = skill.xp;
            PreviousXPAmountImage.fillAmount = skill.xp / (float)skill.xpNext;
            UpdateText.text = newSkillText;
            UpdateText.gameObject.SetActive(true);
            xpGained = 0;
            ThisSkill = skill;
        }

        public void StartAnimation() {
            _time = 0f;
            StartCoroutine(SkillLevelAnimation());
        }

        private IEnumerator SkillLevelAnimation() {
            while (_time <= _duration) {
                SkillLevelSlider.value += Time.fixedDeltaTime * xpGained / _duration;
                if (SkillLevelSlider.value >= SkillLevelSlider.maxValue)
                    LevelUp();
                _time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            SkillLevelSlider.value = ThisSkill.xp;
        }

        private void LevelUp() {
            SkillLevelText.text = $"{ThisSkill.Level}";
            PreviousXPAmountImage.fillAmount = 0f;
            SkillLevelSlider.value = 0f;
            SkillLevelSlider.maxValue = ThisSkill.xpNext;
            UpdateText.text = leveledSkillText;
            UpdateText.gameObject.SetActive(true);
        }
    }
}