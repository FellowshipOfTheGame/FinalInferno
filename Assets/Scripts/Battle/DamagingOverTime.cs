﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DamagingOverTime : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return dmgPerTurn; } }
        private int dmgPerTurn;
        private Element element;

        public DamagingOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur = 1) {
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            dmgPerTurn = Mathf.FloorToInt(Source.curDmg * value);
        }

        public override void Amplify(float modifier){
            dmgPerTurn = Mathf.FloorToInt(modifier * dmgPerTurn);
        }

        public override void Update(){
            Target.TakeDamage(dmgPerTurn, 1.0f, DamageType.None, element);
            base.Update();
        }
    }
}