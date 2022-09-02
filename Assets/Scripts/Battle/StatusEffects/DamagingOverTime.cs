using UnityEngine;

namespace FinalInferno {
    public class DamagingOverTime : StatusEffect {
        public override StatusType Type => StatusType.Undesirable;
        public override float Value => dmgPerTurn;
        private int dmgPerTurn;
        private float valueReceived;
        private Element element;
        public override StatusEffectVisuals VFXID {
            get => element switch {
                Element.Earth => StatusEffectVisuals.Quicksand,
                Element.Fire => StatusEffectVisuals.Burn,
                Element.Water => StatusEffectVisuals.Hypothermia,
                Element.Wind => StatusEffectVisuals.Suffocation,
                Element.Neutral => StatusEffectVisuals.DamageOverTime,
                _ => StatusEffectVisuals.Null,
            };
        }

        public DamagingOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            valueReceived = value;
            dmgPerTurn = Mathf.Max(Mathf.FloorToInt(Source.CurDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DamagingOverTime(Source, target, valueReceived * modifier, element, Duration), true);
        }

        public override void Amplify(float modifier) {
            dmgPerTurn = Mathf.Max(Mathf.FloorToInt(modifier * dmgPerTurn), 1);
        }

        public override bool Update() {
            if (base.Update())
                return true;

            Target.TakeDamage(dmgPerTurn, 1.0f, DamageType.None, element, Source);
            return false;
        }
    }
}