using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Paralyzed : StatusEffect {
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return Duration; } }

        public Paralyzed(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            rollValue = value;
            Failed = !Apply();
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.stuns++;
            return true;
        }

        public override void Remove() {
            Target.stuns--;
        }
    }
}