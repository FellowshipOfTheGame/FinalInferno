using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class ResistanceUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return resValue; } }
        private int resValue;

        public ResistanceUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            resValue = Mathf.FloorToInt(trgt.curMagicDef * value);
            Failed = !Apply();
        }

        public override void Amplify(float modifier){
            Remove();
            resValue = Mathf.FloorToInt(modifier * resValue);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curMagicDef += resValue;
            return true;
        }

        public override void Remove() {
            Target.curMagicDef -= resValue;
        }
    }
}