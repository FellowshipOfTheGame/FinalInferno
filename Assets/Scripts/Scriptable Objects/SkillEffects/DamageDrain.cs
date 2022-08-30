using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DamageDrain", menuName = "ScriptableObject/SkillEffect/DamageDrain")]
    public class DamageDrain : SkillEffect {
        private float DmgDrainMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Drain {DmgDrainMultiplier * 100}% damage for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (IsSourceDrainingTarget(source, target))
                return;
            source.AddEffect(new DrainingDamage(source, target, DmgDrainMultiplier, DebuffDuration));
            target.AddEffect(new DamageDrained(source, target, DmgDrainMultiplier, DebuffDuration));
        }

        private static bool IsSourceDrainingTarget(BattleUnit source, BattleUnit target) {
            foreach (StatusEffect effect in target.effects) {
                if (effect is DamageDrained && effect.Source == source)
                    return true;
            }
            return false;
        }
    }
}