using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Show First Skill")]
    public class ShowFirstSkill : ComponentRequester {
        private SkillList skillListManager;

        public override void Act(StateController controller) {
            if (skillListManager != null) {
                Skill firstSkill = skillListManager.GetFirstSkill();
                if (firstSkill != null) {
                    skillListManager.UpdateSkillDescription(firstSkill);
                    BattleSkillManager.SelectSkill(firstSkill);
                } else {
                    Debug.LogError($"firstSkill null in object {name} of type ShowFirstSkill");
                }
            } else {
                Debug.LogError($"skillListManager null in object {name} of type ShowFirstSkill");
            }
        }

        public override void RequestComponent(GameObject provider) {
            skillListManager = provider.GetComponent<SkillList>();
        }
    }
}