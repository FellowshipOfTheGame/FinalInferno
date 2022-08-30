using System.Collections.Generic;
using FinalInferno.UI.Battle;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Banshee", menuName = "ScriptableObject/Enemy/Banshee")]
    public class Banshee : Enemy {
        protected Skill ParalyzeSkill => skills[1];
        protected override Skill SkillDecision(float percentageNotDefense) {
            float roll = Random.Range(0.0f, 1.0f);
            float chanceUseParalyze = Mathf.Min(1f, percentageNotDefense) / 3f;
            if (roll < chanceUseParalyze && !AllHeroesAreParalised()) {
                return ParalyzeSkill;
            }
            if (roll < percentageNotDefense) {
                return AttackDecision();
            }
            return defenseSkill;
        }

        protected override BattleUnit TargetDecision(List<BattleUnit> targetTeam) {
            List<float> targetingChances = GetUnitTargetingChances(targetTeam);
            float roll = Random.Range(0.0f, 1.0f);
            for (int i = 0; i < targetTeam.Count; i++) {
                bool isParalyzingParalizedTarget = BattleSkillManager.currentSkill == ParalyzeSkill && !targetTeam[i].CanAct;
                if (roll <= targetingChances[i] && !isParalyzingParalizedTarget) {
                    return targetTeam[i];
                }
                roll -= targetingChances[i];
            }
            return targetTeam[targetTeam.Count - 1];
        }
    }
}
