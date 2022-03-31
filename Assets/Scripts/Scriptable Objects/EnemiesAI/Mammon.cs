using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Mammon ", menuName = "ScriptableObject/Enemy/Mammon")]
    public class Mammon : Enemy {
        // OBS.: A IA parte do pressuposto que o Mammon é a unica unidade no time inimigo
        private static int financialSecurity = 0;
        private static bool marketCrashed = false;
        private static float SavingsHPTreshold => (0.75f - (0.25f * financialSecurity));
        private static float GreatDepressionHPTreshold => 1f / 3f;
        private Skill BribeSkill => skills[0];
        private Skill SavingsSkill => skills[1];
        private Skill GreatDepressionSkill => skills[2];

        public override void ResetParameters() {
            financialSecurity = 0;
            marketCrashed = false;
        }

        protected override Skill SkillDecision(float percentageNotDefense) {
            BattleUnit battleUnit = BattleManager.instance.GetTeam(UnitType.Enemy)[0];
            float hpPercent = battleUnit.CurHP / (float)battleUnit.MaxHP;

            if (financialSecurity < 3 && hpPercent <= SavingsHPTreshold) {
                financialSecurity++;
                return SavingsSkill;
            }

            if (!marketCrashed && hpPercent <= GreatDepressionHPTreshold) {
                marketCrashed = true;
                return GreatDepressionSkill;
            }

            float roll = Random.Range(0.0f, 1.0f);
            float percentageDebuff = Mathf.Min(1f, percentageNotDefense) / 3f;
            if (!IsBribeOnCooldown() && roll < percentageDebuff) {
                return BribeSkill;
            }

            if (roll < percentageNotDefense) {
                return AttackDecision();
            }

            return defenseSkill;
        }

        private static bool IsBribeOnCooldown() {
            foreach (BattleUnit unit in BattleManager.instance.GetTeam(UnitType.Hero)) {
                if (!unit.CanAct) {
                    return true;
                }
            }
            return false;
        }
    }
}