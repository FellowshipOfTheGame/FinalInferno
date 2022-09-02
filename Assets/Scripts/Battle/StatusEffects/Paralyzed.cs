﻿namespace FinalInferno {
    public class Paralyzed : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.Paralyzed;
        public override StatusType Type => StatusType.Undesirable;
        public override float Value => Duration;

        public Paralyzed(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            rollValue = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new Paralyzed(Source, target, rollValue * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.stuns++;
            return true;
        }

        public override void Remove() {
            Target.stuns--;
            base.Remove();
        }
    }
}