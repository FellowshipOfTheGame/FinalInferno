using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu
{
    public class LoadSkillDescription : MonoBehaviour
    {
        [SerializeField] private Text skillNameText;
        [SerializeField] private Image skillImage;

        [SerializeField] private Image skillElementImage;
        [SerializeField] private Text skillElementText;

        [SerializeField] private Image skillTargetTypeImage;
        [SerializeField] private Text skillTargetTypeText;

        [SerializeField] private Text skillDescriptionText;

        public void LoadSkillInfo(PlayerSkill skill)
        {
            skillNameText.text = skill.name;
            // skillImage.sprite = skill.

            skillElementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillElementText.text = skill.attribute.ToString();

            skillTargetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillTargetTypeText.text = skill.target.ToString();

            skillDescriptionText.text = skill.description;
        }
    }
}