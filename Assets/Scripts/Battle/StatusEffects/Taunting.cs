using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Taunting : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return aggroIncrease; } }
        private float aggroIncrease;

        public Taunting(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            aggroIncrease = value;
            Apply();
        }

        public override void Amplify(float modifier){
            aggroIncrease *= modifier;
        }

        public override bool Update(){
            Target.aggro += aggroIncrease;
            return base.Update();
        }
    }
}