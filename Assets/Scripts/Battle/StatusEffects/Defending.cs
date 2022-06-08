using UnityEngine;

namespace FinalInferno {
    public class Defending : StatusEffect {
        private const string DefendingAnimString = "IsDefending";
        public override StatusType Type => StatusType.None;
        public override float Value => defValue;
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.Defending;
        private int defValue;
        private int resValue;

        public Defending(BattleUnit trgt, float value, int dur) {
            if (dur < 0)
                dur = 0;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = Target;
            defValue = Mathf.Max(Mathf.FloorToInt(trgt.CurDef * value), 1);
            resValue = Mathf.Max(Mathf.FloorToInt(trgt.CurMagicDef * value), 1);
            Failed = !Apply();
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force) || TargetHasEffectOfType(typeof(Defending)))
                return false;

            Target.CurDef += defValue;
            Target.CurMagicDef += resValue;
            Target.GetComponent<Animator>().SetBool(DefendingAnimString, true);
            return true;
        }

        public override void Remove() {
            Target.CurDef -= defValue;
            Target.CurMagicDef -= resValue;
            Target.GetComponent<Animator>().SetBool(DefendingAnimString, false);
            base.Remove();
        }
    }
}