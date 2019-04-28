using System.Collections;
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
        void Update()
        {
            skill = BattleManager.instance.currentUnit.unit.attackSkill;
        }
    }

}
