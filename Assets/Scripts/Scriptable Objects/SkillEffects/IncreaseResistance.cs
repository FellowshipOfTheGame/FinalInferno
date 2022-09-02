using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "IncreaseResistance", menuName = "ScriptableObject/SkillEffect/IncreaseResistance")]
    public class IncreaseResistance : SkillEffect {
        private float MagicDefUpMultiplier => value1;
        private int BuffDuration => (int)value2;
        public override string Description => $"Increase magical resistance by {MagicDefUpMultiplier * 100}% for {BuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (BuffDuration < 0) {
                target.CurMagicDef += (int)(MagicDefUpMultiplier * target.CurMagicDef);
            } else {
                target.AddEffect(new ResistanceUp(source, target, MagicDefUpMultiplier, BuffDuration));
            }
        }
    }
}