using UnityEngine;

namespace FinalInferno {
    public class LosingHPOverTime : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.LosingHPOverTime;
        public override StatusType Type => StatusType.Undesirable;
        public override float Value => Target.MaxHP * percentageLoss;
        private float percentageLoss;
        private Element element;
        private int damageDealt = 0;

        public LosingHPOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            percentageLoss = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new LosingHPOverTime(Source, target, percentageLoss * modifier, element, Duration), true);
        }

        public override void Amplify(float modifier) {
            percentageLoss *= modifier;
        }

        public override bool Update() {
            if (base.Update())
                return true;

            damageDealt = Mathf.Max(Mathf.FloorToInt(Target.MaxHP * percentageLoss), 1);
            Target.TakeDamage(damageDealt, 1.0f, DamageType.None, element, Source);
            return false;
        }
    }
}