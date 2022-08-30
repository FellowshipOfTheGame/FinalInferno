using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Regen", menuName = "ScriptableObject/SkillEffect/Regen")]
    public class Regen : SkillEffect {
        private float MaxHPMultiplier => value1;
        private int HoTDuration => (int)value2;
        public override string Description => $"Heals {MaxHPMultiplier * 100}% of Target's Max HP every turn for {HoTDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.AddEffect(new Regenerating(source, target, MaxHPMultiplier, HoTDuration));
        }
    }
}