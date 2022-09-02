using UnityEngine;

namespace FinalInferno {
    public class ResistanceUp : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.ResistanceUp;
        public override StatusType Type => StatusType.Buff;
        public override float Value => resValue;
        private int resValue;
        private float valueReceived;

        public ResistanceUp(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
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
            target.AddEffect(new ResistanceUp(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurMagicDef -= resValue;
            resValue = Mathf.Max(Mathf.FloorToInt(modifier * resValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurMagicDef += resValue;
            return true;
        }

        public override void Remove() {
            Target.CurMagicDef -= resValue;
            base.Remove();
        }
    }
}