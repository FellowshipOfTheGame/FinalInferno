using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item que ativa a skill de defesa.
	/// </summary>
    public class DefenseItem : SkillItem
    {
        new void Awake(){
            item.OnEnter += GetSkill;
            base.Awake();
        }
        void GetSkill()
        {
            skill = (BattleManager.instance.currentUnit != null)? BattleManager.instance.currentUnit.unit.defenseSkill : null;
        }
    }

}
