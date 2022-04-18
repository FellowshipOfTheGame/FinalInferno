using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "ShareDebuff", menuName = "ScriptableObject/SkillEffect/ShareDebuff")]
    public class ShareDebuff : SkillEffect {
        private int SharedEffectIndex => (int)value1;
        private float EffectModifier => value2;
        public override string Description => $"Share {EffectModifier * 100}% of selected status effect to allies";

        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach (BattleUnit unit in allies) {
                if (unit != target) {
                    target.effects[Mathf.Clamp(SharedEffectIndex, 0, target.effects.Count - 1)].CopyTo(unit, EffectModifier);
                }
            }
        }
    }
}
