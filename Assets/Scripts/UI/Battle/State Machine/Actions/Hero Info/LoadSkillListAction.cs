using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Skill List")]
    public class LoadSkillListAction : ComponentRequester
    {
        private HeroInfo info;
        private SkillList skillList;

        public override void Act(StateController controller)
        {
            skillList.UpdateContent(info.GetCurrentHeroSkills());
        }

        public override void RequestComponent(GameObject provider)
        {
            HeroInfo newInfo = provider.GetComponent<HeroInfo>();
            SkillList newSkillList = provider.GetComponent<SkillList>();
            if (newInfo != null)
                info = newInfo;
            if (newSkillList != null)
                skillList = newSkillList;
        }
    }

}