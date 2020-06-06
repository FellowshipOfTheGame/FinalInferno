﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item que ativa a skill de ataque.
	/// </summary>
    public class AttackItem : SkillItem
    {
        [SerializeField] private UI.Battle.SkillMenu.SkillList skillListManager;
        new void Awake(){
            item.OnEnter += GetSkill;
            base.Awake();
        }
        void GetSkill()
        {
            skill = (BattleManager.instance.currentUnit != null)? BattleManager.instance.currentUnit.unit.attackSkill : null;
            if(skill){
                skillListManager.UpdateSkillDescription(skill);
                // TO DO: mostrar o console
            }
        }
    }

}
