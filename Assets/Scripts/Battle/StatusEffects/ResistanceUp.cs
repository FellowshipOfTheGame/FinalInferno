﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class ResistanceUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return resValue; } }
        private int resValue;
        private float valueReceived;

        public ResistanceUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            resValue = Mathf.FloorToInt(trgt.curMagicDef * value);
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new ResistanceUp(Source, target, valueReceived * modifier, Duration), true);
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