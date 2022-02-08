using UnityEngine;


namespace FinalInferno {
    [CreateAssetMenu(fileName = "DecreaseResistance", menuName = "ScriptableObject/SkillEffect/DecreaseResistance")]
    public class DecreaseResistance : SkillEffect {
        // value1 = magicDefUp multiplier
        // value2 = buff duration
        public override string Description => "Decrease magical resistance by " + value1 * 100 + "% for " + value2 + " turns";

        public override void Apply(BattleUnit source, BattleUnit target) {
            if (value2 < 0) {
                target.curMagicDef -= (int)value1 * target.curMagicDef;
            } else {
                target.AddEffect(new ResistanceDown(source, target, value1, (int)value2));
            }
        }
    }
}