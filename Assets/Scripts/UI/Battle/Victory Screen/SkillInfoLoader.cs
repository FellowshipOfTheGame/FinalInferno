﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Victory
{
    public class SkillInfoLoader : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image skillElementImage;
        [SerializeField] private Image skillTargetTypeImage;
        [SerializeField] private Text skillDescription;

        public void LoadSkillInfo(PlayerSkill skill)
        {
            skillImage.sprite = skill.skillImage;
            skillElementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            skillTargetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];
            skillDescription.text = skill.description;
        }
    }
}