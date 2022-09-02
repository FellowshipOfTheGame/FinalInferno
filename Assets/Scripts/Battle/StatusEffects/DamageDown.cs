using UnityEngine;

namespace FinalInferno {
    public class DamageDown : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DamageDown;
        public override StatusType Type => StatusType.Debuff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float valueReceived;

        public DamageDown(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            dmgValue = Mathf.Max(Mathf.FloorToInt(trgt.CurDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DamageDown(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurDmg += dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(modifier * dmgValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurDmg -= dmgValue;
            return true;
        }

        public override void Remove() {
            Target.CurDmg += dmgValue;
            base.Remove();
        }
    }
}