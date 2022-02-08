using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "InitiativeBonus", menuName = "ScriptableObject/SkillEffect/InitiativeBonus")]
    public class InitiativeBonus : SkillEffect {
        // value1 = absolute points decrease
        // value2 = percentage speed to decrease points
        public override string Description {
            get {
                string desc = "Decrease action points by ";
                desc += (value1 != 0) ? (value1 + "") : "";
                desc += (value1 != 0 && value2 != 0) ? " plus " : "";
                desc += (value2 != 0) ? ("an amount proportional to " + value2 * 100 + "% speed") : "";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.actionPoints -= Mathf.FloorToInt(value1);
            if (value2 > float.Epsilon || value2 < -float.Epsilon) {
                float bonus = Mathf.Abs(value2) * target.curSpeed;
                // Como espera-se que os valores de value2 estejam entre 0 e 1, o valor minimo foi puxado para baixo
                // A logica descrita abaixo se mantem, mas para os novos valores de minimo e maximo
                float minValue = BattleManager.instance.MinBaseSpeed * 0.5f;
                float maxValue = BattleManager.instance.MaxBaseSpeed;
                if (bonus >= minValue) {
                    // Se o produto de value2 pela velocidade estiver acima da menor velocidade base, o bonus varia linearmente entre o custo base
                    // e a média entre o custo base e custo maximo
                    bonus = Mathf.Clamp(((bonus - minValue) / (float)(maxValue - minValue)), 0f, 1f);
                    bonus *= (((Skill.baseCost + Skill.maxCost) / 2.0f) - Skill.baseCost);
                    bonus += Skill.baseCost;
                } else {
                    // Se o produto de value2 pela velocidade estiver abaixo da menor velocidade base, o bonus varia linearmente entre 0 e o custo base
                    bonus = Mathf.Clamp(bonus / (float)minValue, 0f, 1f) * Skill.baseCost;
                }

                // Debug.Log($"bonus = {bonus}");

                if (value2 > float.Epsilon) {
                    target.actionPoints -= Mathf.Max(Mathf.FloorToInt(bonus), 1);
                } else {
                    target.actionPoints += Mathf.Max(Mathf.FloorToInt(bonus), 1);
                }
            }
            BattleManager.instance.queue.Sort();
        }
    }
}