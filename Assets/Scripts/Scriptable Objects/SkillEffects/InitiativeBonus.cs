using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "InitiativeBonus", menuName = "ScriptableObject/SkillEffect/InitiativeBonus")]
    public class InitiativeBonus : SkillEffect {
        private float AbsolutePointReduction => value1;
        private float SpeedPercentagePointReduction => value2;
        public override string Description {
            get {
                string desc = "Decrease action points by ";
                desc += (AbsolutePointReduction != 0) ? $"{AbsolutePointReduction}" : "";
                desc += (AbsolutePointReduction != 0 && SpeedPercentagePointReduction != 0) ? " plus " : "";
                desc += (SpeedPercentagePointReduction != 0) ? $"an amount proportional to {SpeedPercentagePointReduction * 100}% speed" : "";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.actionPoints -= Mathf.FloorToInt(AbsolutePointReduction);
            if (SpeedPercentagePointReduction > float.Epsilon || SpeedPercentagePointReduction < -float.Epsilon) {
                float absoluteVariation = Mathf.Abs(SpeedPercentagePointReduction) * target.curSpeed;
                float variation = CalculateNormalizedVariation(absoluteVariation);

                if (SpeedPercentagePointReduction > float.Epsilon) {
                    target.actionPoints -= Mathf.Max(Mathf.FloorToInt(variation), 1);
                } else {
                    target.actionPoints += Mathf.Max(Mathf.FloorToInt(variation), 1);
                }
            }
            BattleManager.instance.queue.Sort();
        }

        private static float CalculateNormalizedVariation(float absoluteVariation) {
            float minValue = BattleManager.instance.MinBaseSpeed * 0.5f;
            float maxValue = BattleManager.instance.MaxBaseSpeed;
            if (absoluteVariation < minValue) {
                // Se o produto de SpeedPercentagePointReduction pela velocidade estiver abaixo da menor velocidade base, o bonus varia linearmente entre 0 e o custo base
                return Mathf.Clamp(absoluteVariation / minValue, 0f, 1f) * Skill.baseCost;
            }
            float variation;
            // Se o produto de SpeedPercentagePointReduction pela velocidade estiver acima da menor velocidade base, o bonus varia linearmente entre o custo base
            // e a média entre o custo base e custo maximo
            variation = Mathf.Clamp((absoluteVariation - minValue) / (maxValue - minValue), 0f, 1f);
            variation *= ((Skill.baseCost + Skill.maxCost) / 2.0f) - Skill.baseCost;
            variation += Skill.baseCost;
            return variation;
        }
    }
}