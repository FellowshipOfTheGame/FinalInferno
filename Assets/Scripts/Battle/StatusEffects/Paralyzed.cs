using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Paralyzed : StatusEffect {
        public override StatusType Type { get{ return StatusType.Undesirable; } }
        public override float Value { get{ return Duration; } }
        public override int TurnsLeft { protected set{ base.TurnsLeft = value; } get{ return (base.TurnsLeft > int.MinValue)? base.TurnsLeft : 99;} }

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

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new Paralyzed(Source, target, rollValue * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.stuns++;
            return true;
        }

        public override void Remove() {
            Target.stuns--;
            base.Remove();
        }
    }
}