using UnityEngine;

namespace FinalInferno {
    public class DrainingSpeed : StatusEffect {
        public override StatusType Type => StatusType.Buff;
        public override float Value => spdValue;
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DrainingSpeed;
        private int spdValue;
        private float multiplier;

        public DrainingSpeed(BattleUnit unitDraining, BattleUnit unitDrained, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = unitDraining;
            Source = unitDrained;
            multiplier = value;
            spdValue = Mathf.Max(Mathf.FloorToInt(Source.CurSpeed * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DrainingSpeed(target, Source, multiplier * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurSpeed -= spdValue;
            spdValue = Mathf.Max(Mathf.FloorToInt(modifier * spdValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurSpeed += spdValue;
            return true;
        }

        public override void Remove() {
            Target.CurSpeed -= spdValue;
            base.Remove();
        }
    }
}