using UnityEngine;

namespace FinalInferno {
    public class DamageUpExponential : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DamageUpExponential;
        public override StatusType Type => StatusType.Buff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float valueReceived;

        public DamageUpExponential(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
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
            target.AddEffect(new DamageUpExponential(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurDmg -= dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(dmgValue * modifier), 1);
            Target.CurDmg += dmgValue;
            valueReceived *= modifier;
        }

        public override bool Apply(bool force = false) {
            // Esse status effect não pode ser aplicado mais de uma vez
            if (!base.Apply(force) || TargetHasEffectOfType(typeof(DamageUpExponential)))
                return false;

            IncreaseTargetCurrentDamage();
            return true;
        }

        private void IncreaseTargetCurrentDamage() {
            int increment = Mathf.Max(Mathf.FloorToInt(Target.CurDmg * valueReceived), 1);
            dmgValue += increment;
            Target.CurDmg += increment;
        }

        public override bool Update() {
            if (base.Update()) {
                return true;
            }

            IncreaseTargetCurrentDamage();
            return false;
        }

        public override void Remove() {
            Target.CurDmg -= dmgValue;
            base.Remove();
        }
    }
}