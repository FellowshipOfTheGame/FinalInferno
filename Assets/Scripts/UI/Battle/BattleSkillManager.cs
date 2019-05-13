using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle
{
    public static class BattleSkillManager
    {
        public static Skill currentSkill;
        public static List<BattleUnit> currentTargets = new List<BattleUnit>();

        public static void UseSkill()
        {                
            currentSkill.Use(BattleManager.instance.currentUnit, currentTargets);
            BattleManager.instance.UpdateQueue(currentSkill.cost);

            foreach (BattleUnit target in currentTargets)
                Debug.Log(currentSkill.name + "ing " + target.unit.name + " - " + target.curHP + "/" + target.unit.hpMax);
        }

        public static TargetType GetSkillType()
        {
            return currentSkill != null ? currentSkill.target : TargetType.Null;
        }
    }

}