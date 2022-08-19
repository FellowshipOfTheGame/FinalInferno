using FinalInferno.UI.Victory;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class VictorySkillListItem : AxisInteractableItem {
        [Space]
        protected PlayerSkill skill;
        public SkillInfoLoader loader;

        private new void Awake() {
            base.Awake();
            OnEnter += UpdateSkillDescription;
        }

        private void UpdateSkillDescription() {
            if (!skill)
                skill = GetComponent<UpdatedSkill>().ThisSkill;
            loader.LoadSkillInfo(skill);
        }
    }
}
