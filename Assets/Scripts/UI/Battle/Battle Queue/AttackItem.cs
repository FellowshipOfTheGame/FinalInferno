using UnityEngine;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.Battle.QueueMenu {
    public class AttackItem : SkillItem {
        private const string showConsoleAnimString = "ShowConsole";
        private const string showDetailsAnimString = "ShowSkillDetails";
        [SerializeField] private SkillList skillListManager;
        [SerializeField] private Animator consoleAnim;
        private BattleUnit CurrentUnit => BattleManager.instance.CurrentUnit;

        private new void Awake() {
            item.OnEnter += GetSkill;
            base.Awake();
        }

        private void GetSkill() {
            skill = (CurrentUnit != null) ? CurrentUnit.Unit.attackSkill : null;
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
