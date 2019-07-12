﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DefenseUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;

        public DefenseUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            defValue = Mathf.FloorToInt(trgt.curDef * value);
            Failed = !Apply();
        }

        public override void Amplify(float modifier){
            Remove();
            defValue = Mathf.FloorToInt(modifier * defValue);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDef += defValue;
            return true;
        }

        public override void Remove() {
            Target.curDef -= defValue;
        }
    }
}