using FinalInferno.UI.SkillsMenu;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.AII {
    public class SkillsMenuSkillItem : MonoBehaviour {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private RectTransform rect;
        [SerializeField] private Text skillNameText;
        [SerializeField] private Image skillImage;
        private PlayerSkill skill;
        private LoadSkillDescription loader;
        private HeroSkillsContent content;
        private int heroIndex = 0;

        private void Awake() {
            item.OnEnter += UpdateSkillDescription;
            item.OnEnter += ClampSkillContent;
        }

        public void LoadSkill(PlayerSkill mySkill, LoadSkillDescription _loader, HeroSkillsContent _content, int heroIdx) {
            skill = mySkill;
            loader = _loader;
            content = _content;
            heroIndex = heroIdx;

            skillNameText.text = skill.name;
            skillImage.sprite = skill.skillImage;
        }

        private void UpdateSkillDescription() {
            loader.LoadSkillInfo(skill, heroIndex);
        }

        private void ClampSkillContent() {
            content.ClampContent(rect);
        }
    }
}
