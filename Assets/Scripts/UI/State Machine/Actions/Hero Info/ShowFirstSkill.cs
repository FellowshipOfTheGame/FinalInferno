using System;
using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Show First Skill")]
    public class ShowFirstSkill : ComponentRequester {
        private SkillList skillListManager;

        public override void Act(StateController controller) {
            Skill firstSkill = skillListManager.GetFirstSkill();
            if (firstSkill == null)
                Debug.LogError($"Failed to get first skill from SkillList object {skillListManager}", skillListManager);
            skillListManager.UpdateSkillDescription(firstSkill);
            BattleSkillManager.SelectSkill(firstSkill);
        }

        public override void RequestComponent(GameObject provider) {
            skillListManager = provider.GetComponent<SkillList>();
        }
    }
}