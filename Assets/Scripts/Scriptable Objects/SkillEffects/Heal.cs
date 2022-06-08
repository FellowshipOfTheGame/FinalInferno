using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/SkillEffect/Heal")]
    public class Heal : SkillEffect {
        private float DmgMultiplier => value1;
        private float HealthPercentageHeal => value2;
        public override string Description {
            get {
                string desc = "Increase current HP by ";
                desc += (DmgMultiplier != 0) ? $"{DmgMultiplier * 100}% of user's power" : "";
                desc += (DmgMultiplier != 0 && HealthPercentageHeal != 0) ? " plus " : "";
                desc += (HealthPercentageHeal != 0) ? $"{HealthPercentageHeal * 100}% of target's max HP" : "";
                return desc;
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (DmgMultiplier != 0)
                target.Heal(source.CurDmg, DmgMultiplier, source);

            if (HealthPercentageHeal != 0)
                target.Heal(target.Unit.hpMax, HealthPercentageHeal, source);
        }
    }
}
