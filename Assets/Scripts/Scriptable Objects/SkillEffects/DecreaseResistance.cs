using UnityEngine;


namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreaseResistance", menuName = "ScriptableObject/SkillEffect/DecreaseResistance")]
    public class DecreaseResistance : SkillEffect {
        private float MagicDefDownMultiplier => value1;
        private int DebuffDuration => (int)value2;
        public override string Description => $"Decrease magical resistance by {MagicDefDownMultiplier * 100}% for {DebuffDuration} turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (DebuffDuration < 0) {
                target.curMagicDef -= (int)(MagicDefDownMultiplier * target.curMagicDef);
            } else {
                target.AddEffect(new ResistanceDown(source, target, MagicDefDownMultiplier, DebuffDuration));
            }
        }
    }
}