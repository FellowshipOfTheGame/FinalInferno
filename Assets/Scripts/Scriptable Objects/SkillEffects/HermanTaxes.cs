using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "HermanTaxes", menuName = "ScriptableObject/SkillEffect/HermanTaxes")]
    public class HermanTaxes : SkillEffect {
        private float CopiedBuffsStrength => value1;
        public override string Description => $"Herman receives {CopiedBuffsStrength * 100}% of buffs and heals received by allies";
        [SerializeField] private GameObject visualEffect = null;

        public override void Apply(BattleUnit source, BattleUnit target) {
            List<BattleUnit> allies = BattleManager.instance.GetTeam(target);
            foreach (BattleUnit unit in allies) {
                if (unit == target)
                    continue;
                unit.OnReceiveBuff += PayHerman;
                unit.OnHeal += FeedHerman;
            }
        }

        public void FeedHerman(BattleUnit target, List<BattleUnit> units, bool shouldOverride1 = false, float val1 = 0f, bool shouldOverride2 = false, float val2 = 0f) {
            if (units.Count < 1)
                return;

            BattleUnit herman = FindHerman(target);
            if (!herman)
                return;
            herman.Heal((int)val1, CopiedBuffsStrength);
            ShowVFX(herman);
        }

        private static BattleUnit FindHerman(BattleUnit target) {
            return BattleManager.instance.GetTeam(target).Find(unit => unit.Unit.name == "Herman");
        }

        private void ShowVFX(BattleUnit herman) {
            if (!visualEffect)
                return;
            GameObject obj = Instantiate(visualEffect, herman.transform);
            obj.GetComponent<SkillVFX>().SetTargetCallback(herman);
        }

        public void PayHerman(BattleUnit target, List<BattleUnit> units, bool shouldOverride1 = false, float val1 = 0f, bool shouldOverride2 = false, float val2 = 0f) {
            if (units.Count < 1)
                return;

            BattleUnit herman = FindHerman(target);
            if (!herman)
                return;
            int statusEffectIndex = Mathf.Clamp((int)val1, 0, target.effects.Count - 1);
            target.effects[statusEffectIndex].CopyTo(herman, CopiedBuffsStrength);
            ShowVFX(herman);
        }
    }
}
