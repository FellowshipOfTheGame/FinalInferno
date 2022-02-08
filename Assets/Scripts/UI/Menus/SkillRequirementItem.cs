using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu {
    public class SkillRequirementItem : MonoBehaviour {
        [SerializeField] private Image icon;
        [SerializeField] private Text skillName;
        [SerializeField] private Text levelText;

        public void ShowRequirement(PlayerSkill skill, int level) {
            icon.sprite = skill.skillImage;
            skillName.text = skill.name;
            levelText.text = "Level ";
            levelText.text += (skill.Level >= level) ? "<color=#006400>" : "<color=#840000>";
            levelText.text += string.Format("{0,2}", level) + "</color>";
        }
    }
}