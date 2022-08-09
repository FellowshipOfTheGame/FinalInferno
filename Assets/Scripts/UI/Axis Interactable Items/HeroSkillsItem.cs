using FinalInferno.UI.FSM;
using FinalInferno.UI.SkillsMenu;
using UnityEngine;

namespace FinalInferno.UI.AII {
    public class HeroSkillsItem : MonoBehaviour {
        [SerializeField] private AxisInteractableItem item;
        [SerializeField] private AIIManager skillsManager;
        [SerializeField] private ButtonClickDecision cancelDecision;

        [SerializeField] private int index;
        [SerializeField] private SkillsContent content;

        private bool isCurrent = false;

        private void Awake() {
            item.OnEnter += EnableFirstSkillDescription;
            item.OnEnter += UpdateSkillsContentPosition;
            item.OnEnter += () => isCurrent = true;
            item.OnExit += () => isCurrent = false;
            item.OnExit += DisableSkills;
            item.OnAct += OnCancel;
        }

        public void OnCancel() {
            if (cancelDecision)
                cancelDecision.Click();
        }

        public void LoseFocus() {
            if (isCurrent)
                skillsManager.SetFocus(false);
        }

        public void RegainFocus() {
            if (isCurrent)
                skillsManager.SetFocus(true);
        }

        private void EnableFirstSkillDescription() {
            skillsManager.Activate();
        }

        private void DisableSkills() {
            skillsManager.Deactivate();
        }

        private void UpdateSkillsContentPosition() {
            content.SetContentToPosition(index);
        }
    }
}
