﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DamageDrained : StatusEffect {
        public override StatusEffectVisuals VFXID { get => StatusEffectVisuals.DamageDrained; }
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return dmgValue; } }
        private int dmgValue;
        private float valueReceived;

        public DamageDrained(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            dmgValue = Mathf.Max(Mathf.FloorToInt(trgt.curDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DamageDrained(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Target.curDmg += dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(modifier * dmgValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDmg -= dmgValue;
            return true;
        }

        public override void Remove() {
            Target.curDmg += dmgValue;
            base.Remove();
        }
    }
}