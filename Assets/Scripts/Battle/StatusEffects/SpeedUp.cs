using UnityEngine;

namespace FinalInferno {
    public class SpeedUp : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.SpeedUp;
        public override StatusType Type => StatusType.Buff;
        public override float Value => speedValue;
        private int speedValue;
        private float valueReceived;

        public SpeedUp(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            speedValue = Mathf.Max(Mathf.FloorToInt(trgt.CurSpeed * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new SpeedUp(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurSpeed -= speedValue;
            speedValue = Mathf.Max(Mathf.FloorToInt(modifier * speedValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurSpeed += speedValue;
            return true;
        }

        public override void Remove() {
            Target.CurSpeed -= speedValue;
            base.Remove();
        }
    }
}