using UnityEngine;

namespace FinalInferno {
    public class MarketCrash : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.MarketCrash;
        public override StatusType Type => StatusType.None;
        public override float Value => currentReduction;
        private float currentReduction;
        private float dmgMultiplier;
        private DamageType dmgType;
        private Element element;
        private int turnCount;

        public MarketCrash(BattleUnit mammon, BattleUnit hero, float dmgMult = 0.1f, DamageType type = DamageType.None, Element ele = Element.Neutral) {
            Duration = int.MinValue;
            TurnsLeft = Duration;
            turnCount = 0;
            currentReduction = 0.01f;
            dmgType = type;
            element = ele;
            dmgMultiplier = dmgMult;
            Source = mammon;
            Target = hero;
            Failed = !Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force) || SourceHasEffectOfType(typeof(MarketCrash)))
                return false;

            Target.HealResistance += currentReduction;
            Source.DamageResistance += currentReduction;
            return true;
        }

        public override bool Update() {
            if (base.Update())
                return true;

            UpdateDamageAndHealReductions();
            Target.TakeDamage(Source.CurDmg, dmgMultiplier, dmgType, element, Source);
            return false;
        }

        private void UpdateDamageAndHealReductions() {
            turnCount++;
            Target.HealResistance -= currentReduction;
            Source.DamageResistance -= currentReduction;
            currentReduction = Mathf.Clamp(Mathf.Pow(1.01f, turnCount + 1) - 1f, 0f, 1f);
            Target.HealResistance += currentReduction;
            Source.DamageResistance += currentReduction;
        }

        public override void Remove() {
            Target.HealResistance -= currentReduction;
            Source.DamageResistance -= currentReduction;
            Source.RemoveEffect(this);
            Removed = true;
        }
    }
}