using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory
{
    public class SkillInfoLoader : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Text skillDescription;

        public void LoadSkillInfo(PlayerSkill skill)
        {
            // skillImage.sprite = skill.
            skillDescription.text = skill.description;
        }
    }
}