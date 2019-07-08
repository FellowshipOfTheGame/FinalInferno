using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.Battle
{
    public static class BattleSkillManager
    {
        public static Skill currentSkill;
        public static BattleUnit currentUser;
        public static List<BattleUnit> currentTargets = new List<BattleUnit>();

        public static void UseSkill()
        {
            currentSkill.Use(currentUser, currentTargets);
            SkillVFX.nTargets = currentTargets.Count;
            foreach(BattleUnit target in currentTargets){
                GameObject.Instantiate(currentSkill.VisualEffect, target.transform);
            }
        }

        public static TargetType GetSkillType()
        {
            return currentSkill != null ? currentSkill.target : TargetType.Null;
        }
    }

}