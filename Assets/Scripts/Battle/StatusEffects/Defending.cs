using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Defending : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;

        public Defending(BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = 1;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = Target;
            defValue = Mathf.FloorToInt(trgt.curDef * value);
            Failed = !Apply();
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDef += defValue;
            return false;
        }

        public override void Remove() {
            Target.curDef -= defValue;
        }
    }
}