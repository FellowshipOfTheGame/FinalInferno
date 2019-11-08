using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "LevelUpSkills", menuName = "ScriptableObject/SkillEffect/LevelUpSkills")]
    public class LevelUpSkills : SkillEffect {
        // value1 = dmg multiplier
        public override string Description { get { return "Level up skills "; } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            foreach (EnemySkill skill in team[0].unit.skills){
                skill.LevelUp();
            }
        }
    }
}
