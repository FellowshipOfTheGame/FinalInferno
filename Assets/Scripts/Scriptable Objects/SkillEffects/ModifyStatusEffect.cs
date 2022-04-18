using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "ModifyStatusEffect", menuName = "ScriptableObject/SkillEffect/ModifyStatusEffect")]
    public class ModifyStatusEffect : SkillEffect {
        private int ModifiedEffectIndex => (int)value1;
        private float EffectModifier => value2;
        public override string Description => "Modify effect selected by " + EffectModifier + "x modifier";

        public override void Apply(BattleUnit source, BattleUnit target) {
            target.effects[Mathf.Clamp(ModifiedEffectIndex, 0, target.effects.Count - 1)].Amplify(EffectModifier);
        }
    }
}
