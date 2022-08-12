using FinalInferno.UI.Victory;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class VictorySkillListItem : MonoBehaviour {
        [SerializeField] private AxisInteractableItem item;
        protected PlayerSkill skill;
        public SkillInfoLoader loader;

        private void Awake() {
            item.OnEnter += UpdateSkillDescription;
        }

        private void UpdateSkillDescription() {
            if (!skill)
                skill = GetComponent<UpdatedSkill>().thisSkill;
            loader.LoadSkillInfo(skill);
        }
    }
}
