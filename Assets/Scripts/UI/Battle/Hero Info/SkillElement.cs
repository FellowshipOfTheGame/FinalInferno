using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class SkillElement : AxisInteractableItem {
        [Header("SkillElement")]
        [SerializeField] private Text text;
        [SerializeField] private Image icon;
        [SerializeField] private SkillListItem skillListItem;
        [SerializeField] private SkillItem skillItem;
        public PlayerSkill Skill { get; private set; }

        public void Configure(SkillList skillList, PlayerSkill playerSkill) {
            Skill = playerSkill;
            text.text = Skill.name;
            icon.sprite = Skill.skillImage;
            skillListItem.skillList = skillList;
            skillItem.skill = playerSkill;
        }
    }
}