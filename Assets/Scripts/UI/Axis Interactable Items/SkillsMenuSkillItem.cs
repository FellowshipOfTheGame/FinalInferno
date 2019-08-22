using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.SkillsMenu;
using UnityEngine.UI;

namespace FinalInferno.UI.AII
{
    public class SkillsMenuSkillItem : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem item;
        private PlayerSkill skill;
        private LoadSkillDescription loader;

        [SerializeField] private Text SkillNameText;
        [SerializeField] private Image SkillImage;

        void Awake()
        {
            item.OnEnter += UpdateSkillDescription;
        }

        public void LoadSkill(PlayerSkill mySkill, LoadSkillDescription _loader)
        {
            skill = mySkill;
            loader = _loader;

            SkillNameText.text = skill.name;
            // SkillImage.sprite = skill.
        }

        private void UpdateSkillDescription()
        {
            loader.LoadSkillInfo(skill);
        }
    }
}
