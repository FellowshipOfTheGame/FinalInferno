﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.Battle.SkillMenu;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Show First Skill")]
    public class ShowFirstSkill : ComponentRequester
    {
        private SkillList skillListManager;

        public override void Act(StateController controller){
            if(skillListManager != null){
                Skill firstSkill = skillListManager.GetFirstSkill();
                if(firstSkill != null){
                    skillListManager.UpdateSkillDescription(firstSkill);
                    FinalInferno.UI.Battle.BattleSkillManager.currentSkill = firstSkill;
                }else{
                    Debug.LogError($"firstSkill null in object {name} of type ShowFirstSkill");
                }
            }else{
                Debug.LogError($"skillListManager null in object {name} of type ShowFirstSkill");
            }
        }

        public override void RequestComponent(GameObject provider){
            skillListManager = provider.GetComponent<SkillList>();
        }
    }
}