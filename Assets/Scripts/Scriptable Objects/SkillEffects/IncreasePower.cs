using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreasePower", menuName = "ScriptableObject/SkillEffect/IncreasePower")]
    public class IncreasePower : SkillEffect {
        private float DmgUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase power by {DmgUpMultiplier * 100}% for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (BuffDuration < 0) {
                target.CurDmg += (int)(DmgUpMultiplier * target.CurDmg);
            } else {
                target.AddEffect(new DamageUp(source, target, DmgUpMultiplier, BuffDuration));
            }
        }
    }
}