using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreasePower", menuName = "ScriptableObject/SkillEffect/DecreasePower")]
    public class DecreasePower : SkillEffect {
        private float DmgDownMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Decrease power by {DmgDownMultiplier * 100}% for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (DebuffDuration < 0) {
                target.CurDmg -= (int)(DmgDownMultiplier * target.CurDmg);
            } else {
                target.AddEffect(new DamageDown(source, target, DmgDownMultiplier, DebuffDuration));
            }
        }
    }
}