using UnityEngine;

namespace FinalInferno.UI.Battle.SkillMenu {
    public class DefenseItem : SkillItem {
        private const string showConsoleAnimString = "ShowConsole";
        private const string showDetailsAnimString = "ShowSkillDetails";
        [SerializeField] private SkillMenu.SkillList skillListManager;
        [SerializeField] private Animator consoleAnim;
        private BattleUnit CurrentUnit => BattleManager.instance.CurrentUnit;

        private new void Awake() {
            item.OnEnter += GetSkill;
            base.Awake();
        }

        private void GetSkill() {
            skill = (CurrentUnit != null) ? CurrentUnit.Unit.defenseSkill : null;
            if (skill == null)
                return;
            skillListManager.UpdateSkillDescription(skill);
            ShowConsoleWithSkillDetails();
        }

        private void ShowConsoleWithSkillDetails() {
            if (!consoleAnim)
                return;
            consoleAnim.SetTrigger(showConsoleAnimString);
            consoleAnim.SetTrigger(showDetailsAnimString);
        }
    }
}
