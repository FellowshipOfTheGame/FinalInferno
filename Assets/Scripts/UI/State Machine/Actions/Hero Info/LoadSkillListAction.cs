using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Skill List")]
    public class LoadSkillListAction : ComponentRequester {
        private HeroInfo info;
        private SkillList skillList;

        public override void Act(StateController controller) {
            skillList.UpdateSkillsContent(info.GetCurrentHeroSkills());
        }

        public override void RequestComponent(GameObject provider) {
            if (provider.TryGetComponent(out HeroInfo newInfo))
                info = newInfo;
            if (provider.TryGetComponent(out SkillList newSkillList))
                skillList = newSkillList;
        }
    }
}