using UnityEngine;

namespace FinalInferno {
    public class DrainingDamage : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DrainingDamage;
        public override StatusType Type => StatusType.Buff;
        public override float Value => dmgValue;
        private int dmgValue;
        private float multiplier;
        private bool doubleEdged;
        private bool isPermanent;

        public DrainingDamage(BattleUnit unitDraining, BattleUnit unitDrained, float value, int dur, bool force = false, bool isDoubleEdged = false, bool canBePermanent = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = unitDraining;
            Source = unitDrained;
            doubleEdged = isDoubleEdged;
            multiplier = value;
            isPermanent = canBePermanent;
            dmgValue = Mathf.Max(Mathf.FloorToInt(Source.CurDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DrainingDamage(target, Source, multiplier * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.CurDmg -= dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(modifier * dmgValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.CurDmg += dmgValue;
            return true;
        }

        public override void Remove() {
            if (!isPermanent || Source.CurHP > 0) {
                Target.CurDmg -= dmgValue;
                CheckDoubleEdgedDrain();
            }
            base.Remove();
        }

        private void CheckDoubleEdgedDrain() {
            if (!doubleEdged || Source.CurHP <= 0)
                return;

            GetMyDebuff()?.Remove();
            ApplyInvertedDamageDrain();
        }

        private DamageDrained GetMyDebuff() {
            return (DamageDrained)Source.effects.Find(debuff => debuff.GetType() == typeof(DamageDrained) && debuff.Source == Target && debuff.Target == Source);
        }

        private void ApplyInvertedDamageDrain() {
            Source.AddEffect(new DrainingDamage(Source, Target, multiplier, Duration));
            Target.AddEffect(new DamageDrained(Source, Target, multiplier, Duration));
        }

        public override void ForceRemove() {
            Target.CurDmg -= dmgValue;
            base.Remove();
        }
    }
}