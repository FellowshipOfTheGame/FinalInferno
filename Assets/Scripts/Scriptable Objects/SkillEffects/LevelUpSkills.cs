using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "LevelUpSkills", menuName = "ScriptableObject/SkillEffect/LevelUpSkills")]
    public class LevelUpSkills : SkillEffect {
        public override string Description0 { get { return "Level up skills "; } }
        // value1 = dmg multiplier
        public override string Description1{ get{ return "x Damage"; } }
        public override string Description2{ get{ return null; } }
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            foreach (EnemySkill skill in team[0].unit.skills){
                skill.LevelUp();
            }
        }
    }
}
