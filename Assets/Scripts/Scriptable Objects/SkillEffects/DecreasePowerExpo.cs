using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreasePowerExpo", menuName = "ScriptableObject/SkillEffect/DecreasePowerExponential")]
    public class DecreasePowerExpo : SkillEffect {
        private float DmgDownMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Decrease power by {DmgDownMultiplier * 100}% every turn for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamageDownExponential(source, target, DmgDownMultiplier, DebuffDuration));
        }
    }
}