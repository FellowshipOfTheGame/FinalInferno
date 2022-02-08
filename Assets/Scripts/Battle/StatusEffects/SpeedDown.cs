using UnityEngine;

namespace FinalInferno {
    public class SpeedDown : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.SpeedDown;
        public override StatusType Type => StatusType.Debuff;
        public override float Value => speedValue;
        private int speedValue;
        private float valueReceived;

        public SpeedDown(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if (dur < 0) {
                dur = int.MinValue;
            }

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            speedValue = Mathf.Max(Mathf.FloorToInt(trgt.curSpeed * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new SpeedDown(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.curSpeed += speedValue;
            speedValue = Mathf.Max(Mathf.FloorToInt(modifier * speedValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force)) {
                return false;
            }

            Target.curSpeed -= speedValue;
            return true;
        }

        public override void Remove() {
            Target.curSpeed += speedValue;
            base.Remove();
        }
    }
}