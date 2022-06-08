using UnityEngine;

namespace FinalInferno {
    public class DamageDownExponential : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DamageDownExponential;
        public override StatusType Type => StatusType.Debuff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float valueReceived;

        public DamageDownExponential(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            dmgValue = 0;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DamageDownExponential(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurDmg += dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(dmgValue * modifier), 1);
            Target.CurDmg -= dmgValue;
            valueReceived *= modifier;
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force) || TargetHasEffectOfType(typeof(DamageDownExponential)))
                return false;

            ReduceTargetCurrentDamage();
            return true;
        }

        private void ReduceTargetCurrentDamage() {
            int decrement = Mathf.Max(Mathf.FloorToInt(Target.CurDmg * valueReceived), 1);
            dmgValue += decrement;
            Target.CurDmg -= decrement;
        }

        public override bool Update() {
            if (base.Update())
                return true;

            ReduceTargetCurrentDamage();
            return false;
        }

        public override void Remove() {
            Target.CurDmg += dmgValue;
            base.Remove();
        }
    }
}