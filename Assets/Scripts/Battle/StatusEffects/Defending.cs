using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Defending : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return defValue; } }
        private int defValue;
        private int resValue;

        public Defending(BattleUnit trgt, float value, int dur = 0) {
            if(dur < 0)
                dur = 0;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = Target;
            defValue = Mathf.Max(Mathf.FloorToInt(trgt.curDef * value), 1);
            resValue = Mathf.Max(Mathf.FloorToInt(trgt.curMagicDef * value), 1);
            Failed = !Apply();
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDef += defValue;
            Target.curMagicDef += resValue;
            Target.GetComponent<Animator>().SetBool("IsDefending", true);
            return true;
        }

        public override void Remove() {
            Target.curDef -= defValue;
            Target.curMagicDef -= resValue;
            Target.GetComponent<Animator>().SetBool("IsDefending", false);
            base.Remove();
        }
    }
}