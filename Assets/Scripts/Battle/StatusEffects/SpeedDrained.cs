using UnityEngine;

namespace FinalInferno {
    public class SpeedDrained : StatusEffect {
        public override StatusType Type => StatusType.Debuff;
        public override float Value => spdValue;
        private int spdValue;
        private float valueReceived;

        public SpeedDrained(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if (dur < 0) {
                dur = int.MinValue;
            }

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            spdValue = Mathf.Max(Mathf.FloorToInt(trgt.curSpeed * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new SpeedDrained(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.curSpeed += spdValue;
            spdValue = Mathf.Max(Mathf.FloorToInt(modifier * spdValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force)) {
                return false;
            }

            Target.curSpeed -= spdValue;
            return true;
        }

        public override void Remove() {
            Target.curSpeed += spdValue;
            base.Remove();
        }
    }
}