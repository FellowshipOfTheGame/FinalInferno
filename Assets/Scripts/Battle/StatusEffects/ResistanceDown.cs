using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class ResistanceDown : StatusEffect {
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return resValue; } }
        private int resValue;
        private float valueReceived;

        public ResistanceDown(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            resValue = Mathf.Max(Mathf.FloorToInt(trgt.curMagicDef * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new ResistanceDown(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Target.curMagicDef += resValue;
            resValue = Mathf.Max(Mathf.FloorToInt(modifier * resValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curMagicDef -= resValue;
            return true;
        }

        public override void Remove() {
            Target.curMagicDef += resValue;
            base.Remove();
        }
    }
}