﻿using System.Collections;
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
        void Update()
        {
            skill = BattleManager.instance.currentUnit.unit.defenseSkill;
        }
    }

}
