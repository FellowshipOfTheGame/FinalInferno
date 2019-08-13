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

        public void LoadUpdatedSkill(PlayerSkill skill)
        {
            SkillName.text = skill.name;
            SkillLevelText.text = skill.Level.ToString();
            SkillLevelSlider.maxValue = skill.xpNext;
            SkillLevelSlider.value = skill.xp;
        }
    }
}