﻿namespace FinalInferno {
    public class StatusResistUp : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.StatusResistUp;
        public override StatusType Type => StatusType.Buff;
        public override float Value => resistIncrease;
        private float resistIncrease;

        public StatusResistUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if (dur < 0) {
                dur = int.MinValue;
            }

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            resistIncrease = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new StatusResistUp(Source, target, resistIncrease * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.statusResistance -= resistIncrease;
            resistIncrease *= modifier;
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force)) {
                return false;
            }

            Target.statusResistance += resistIncrease;
            return true;
        }

        public override void Remove() {
            Target.statusResistance -= resistIncrease;
            base.Remove();
        }
    }
}