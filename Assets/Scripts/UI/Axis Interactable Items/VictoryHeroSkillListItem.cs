using UnityEngine;

namespace FinalInferno.UI.AII {
    public class VictoryHeroSkillListItem : MonoBehaviour {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private AIIManager skillsManager;

        private void Awake() {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnExit += DisableSkills;
        }

        private void EnableFirstSkillDescription() {
            skillsManager.Active();
        }

        private void DisableSkills() {
            skillsManager.Deactive();
        }
    }
}
