using UnityEngine;

namespace FinalInferno {
    public class DamageDownExponential : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DamageDownExponential;
        public override StatusType Type => StatusType.Debuff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float valueReceived;

        public DamageDownExponential(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if (dur < 0) {
                dur = int.MinValue;
            }

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
            Target.curDmg += dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(dmgValue * modifier), 1);
            Target.curDmg -= dmgValue;

            valueReceived *= modifier;
        }

        public override bool Apply(bool force = false) {
            // Esse status effect não pode ser aplicado mais de uma vez
            if (!base.Apply(force) || Target.effects.Find(effect => effect.GetType() == typeof(DamageDownExponential)) != null) {
                return false;
            }

            int decrement = Mathf.Max(Mathf.FloorToInt(Target.curDmg * valueReceived), 1);
            dmgValue += decrement;
            Target.curDmg -= dmgValue;
            return true;
        }

        public override bool Update() {
            if (base.Update()) {
                return true;
            }

            int decrement = Mathf.Max(Mathf.FloorToInt(Target.curDmg * valueReceived), 1);
            dmgValue += decrement;
            Target.curDmg -= decrement;
            return false;
        }

        public override void Remove() {
            Target.curDmg += dmgValue;
            base.Remove();
        }
    }
}