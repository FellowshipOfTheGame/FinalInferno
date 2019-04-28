using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.Battle.QueueMenu
{
    /// <summary>
	/// Item da lista de skills.
	/// </summary>
    public class AttackItem : SkillItem
    {
        void Update()
        {
            skill = BattleManager.instance.currentUnit.unit.attackSkill;
        }
    }

}
