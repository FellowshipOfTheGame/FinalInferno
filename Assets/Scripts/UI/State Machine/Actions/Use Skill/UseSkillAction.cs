using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Use Skill")]
    public class UseSkillAction : Action {
        public override void Act(StateController controller) {
            BattleManager.instance.CurrentUnit.SkillSelected();
        }
    }
}