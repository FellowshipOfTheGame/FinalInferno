﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DefenseDown : StatusEffect {
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;

        public DefenseDown(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            defValue = Mathf.FloorToInt(trgt.curDef * value);
            Apply();
        }

        public override void Amplify(float modifier){
            Remove();
            defValue = Mathf.FloorToInt(modifier * defValue);
            Apply();
        }

        public override void Apply() {
            Target.curDef -= defValue;
        }

        public override void Remove() {
            Target.curDef += defValue;
        }
    }
}