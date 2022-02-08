using UnityEngine;

namespace FinalInferno {
    public class DamageUp : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DamageUp;
        public override StatusType Type => StatusType.Buff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float valueReceived;

        public DamageUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if (dur < 0) {
                dur = int.MinValue;
            }

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            dmgValue = Mathf.Max(Mathf.FloorToInt(trgt.curDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DamageUp(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.curDmg -= dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(modifier * dmgValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force)) {
                return false;
            }

            Target.curDmg += dmgValue;
            return true;
        }

        public override void Remove() {
            Target.curDmg -= dmgValue;
            base.Remove();
        }
    }
}