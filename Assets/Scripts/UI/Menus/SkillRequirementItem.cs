using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.SkillsMenu {
    public class SkillRequirementItem : MonoBehaviour {
        private const string redColorString = "<color=#840000>";
        private const string greenColorString = "<color=#006400>";
        [SerializeField] private Image icon;
        [SerializeField] private Text skillName;
        [SerializeField] private Text levelText;

        public void ShowRequirement(PlayerSkill skill, int level) {
            icon.sprite = skill.skillImage;
            skillName.text = skill.name;
            levelText.text = GetLevelTextString(skill, level);
        }

        private string GetLevelTextString(PlayerSkill skill, int level) {
            StringBuilder stringBuilder = new StringBuilder("Level ");
            stringBuilder.Append((skill.Level >= level) ? greenColorString : redColorString);
            stringBuilder.Append($"{string.Format("{0,2}", level)}</color>");
            return stringBuilder.ToString();
        }
    }
}