﻿using UnityEngine;

namespace FinalInferno {
    public class ResistanceDown : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.ResistanceDown;
        public override StatusType Type => StatusType.Debuff;
        public override float Value => resValue;
        private int resValue;
        private float valueReceived;

        public ResistanceDown(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            resValue = Mathf.Max(Mathf.FloorToInt(trgt.CurMagicDef * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new ResistanceDown(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurMagicDef += resValue;
            resValue = Mathf.Max(Mathf.FloorToInt(modifier * resValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurMagicDef -= resValue;
            return true;
        }

        public override void Remove() {
            Target.CurMagicDef += resValue;
            base.Remove();
        }
    }
}