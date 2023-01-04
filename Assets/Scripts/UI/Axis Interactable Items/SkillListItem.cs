using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class SkillListItem : MonoBehaviour {
        public SkillList skillList;
        private PlayerSkill skill;
        private PlayerSkill Skill {
            get {
                if (!skill)
                    skill = GetComponent<SkillElement>().Skill;
                return skill;
            }
            set => skill = value;
        }
        private RectTransform rect;
        [SerializeField] private AxisInteractableItem item;

        private void Awake() {
            rect = GetComponent<RectTransform>();
            item.OnEnter += UpdateSkillDescription;
            item.OnEnter += ClampSkillContent;
        }

        private void UpdateSkillDescription() {
            skillList.UpdateSkillDescription(Skill);
        }

        private void ClampSkillContent() {
            skillList.ClampSkillContent(rect);
        }
    }

}
