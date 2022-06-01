using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public class Bribed : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.Bribed;
        public override StatusType Type => StatusType.Undesirable;
        public override float Value => Duration;

        public Bribed(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
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
            target.AddEffect(new Bribed(Source, target, rollValue * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;
            Target.stuns++;
            return true;
        }

        public override bool Update() {
            if (base.Update())
                return true;

            if (IsFirstEffectInList())
                AttackAlliesWithDmgNerf();
            return false;
        }

        private bool IsFirstEffectInList() {
            return Target.effects.Find(effect => effect.GetType() == typeof(Bribed)) == this;
        }

        private void AttackAlliesWithDmgNerf() {
            Target.SkillSelected();
            List<BattleUnit> allies = BattleManager.instance.GetTeam(Target);
            int teamSize = BattleManager.instance.GetTeam(Target, true).Count;
            int dmgDecrease = Mathf.FloorToInt((teamSize - 1) / (float)teamSize * Target.curDmg);
            Target.curDmg -= dmgDecrease;
            Target.Unit.attackSkill.UseCallbackOrDelayed(Target, allies);
            Target.curDmg += dmgDecrease;
        }

        public override void Remove() {
            Target.stuns--;
            base.Remove();
        }
    }
}