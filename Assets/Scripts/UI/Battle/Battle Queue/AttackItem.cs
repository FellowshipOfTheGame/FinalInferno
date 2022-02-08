using UnityEngine;

namespace FinalInferno.UI.Battle.QueueMenu {
    /// <summary>
	/// Item que ativa a skill de ataque.
	/// </summary>
    public class AttackItem : SkillItem {
        [SerializeField] private UI.Battle.SkillMenu.SkillList skillListManager;
        [SerializeField] private Animator consoleAnim;

        private new void Awake() {
            item.OnEnter += GetSkill;
            base.Awake();
        }

        private void GetSkill() {
            skill = (BattleManager.instance.currentUnit != null) ? BattleManager.instance.currentUnit.Unit.attackSkill : null;
            if (skill != null) {
                skillListManager.UpdateSkillDescription(skill);
                // Mostra o console e pede preview de skill
                if (consoleAnim) {
                    consoleAnim.SetTrigger("ShowConsole");
                    consoleAnim.SetTrigger("ShowSkillDetails");
                }
            }
        }
    }

}
