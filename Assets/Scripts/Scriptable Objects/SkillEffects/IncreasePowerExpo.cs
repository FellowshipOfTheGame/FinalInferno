using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreasePowerExpo", menuName = "ScriptableObject/SkillEffect/IncreasePowerExponential")]
    public class IncreasePowerExpo : SkillEffect {
        private float DmgUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase power by {DmgUpMultiplier * 100}% every turn for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new DamageUpExponential(source, target, DmgUpMultiplier, BuffDuration));
        }
    }
}