using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "LevelUpSkills", menuName = "ScriptableObject/SkillEffect/LevelUpSkills")]
    public class LevelUpSkills : SkillEffect {
        public override string Description => "Level up skills";
        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> team = BattleManager.instance.GetTeam(UnitType.Enemy);
            foreach (EnemySkill skill in team[0].Unit.skills) {
                skill.LevelUp();
            }
        }
    }
}
