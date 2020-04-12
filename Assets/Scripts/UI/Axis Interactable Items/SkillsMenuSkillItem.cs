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
        private int heroIndex = 0;

        [SerializeField] private RectTransform rect;
        [SerializeField] private HeroSkillsContent content;

        [SerializeField] private Text SkillNameText;
        [SerializeField] private Image SkillImage;

        void Awake()
        {
            item.OnEnter += UpdateSkillDescription;
            item.OnEnter += ClampSkillContent;
        }

        public void LoadSkill(PlayerSkill mySkill, LoadSkillDescription _loader, HeroSkillsContent _content, int heroIdx)
        {
            skill = mySkill;
            loader = _loader;
            content = _content;
            heroIndex = heroIdx;

            SkillNameText.text = skill.name;
            SkillImage.sprite = skill.skillImage;
        }

        private void UpdateSkillDescription()
        {
            loader.LoadSkillInfo(skill, heroIndex);
        }

        private void ClampSkillContent()
        {
            content.ClampContent(rect);
        }
    }
}
