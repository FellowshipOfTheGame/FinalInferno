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
            // Esse status effect não pode ser aplicado mais de uma vez
            // No caso de market crash ele fica na lista do Source e não do Target
            if (!base.Apply(force) || Source.effects.Find(effect => effect.GetType() == typeof(MarketCrash)) != null) {
                return false;
            }
            Target.healResistance += currentReduction;
            // Está sobrescrevendo outras alterações na redução de dano
            Source.damageResistance = currentReduction;
            return true;
        }

        public override bool Update() {
            if (base.Update()) {
                return true;
            }

            turnCount++;
            Target.healResistance -= currentReduction;
            currentReduction = Mathf.Clamp(Mathf.Pow(1.01f, turnCount + 1) - 1f, 0f, 1f);
            Target.healResistance += currentReduction;
            // Está sobrescrevendo outras alterações na redução de dano
            Source.damageResistance = currentReduction;

            Target.TakeDamage(Source.curDmg, dmgMultiplier, dmgType, element, Source);

            return false;
        }

        public override void Remove() {
            Target.healResistance -= currentReduction;
            Source.damageResistance = 0f;
            base.Remove();
        }
    }
}