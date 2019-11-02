using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Taunting : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return aggroIncrease; } }
        public override int TurnsLeft { protected set{ base.TurnsLeft = value; } get{ return (base.TurnsLeft > int.MinValue)? base.TurnsLeft-1 : 99;} }
        private float aggroIncrease;

        public Taunting(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            aggroIncrease = value;
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new Taunting(Source, target, aggroIncrease * modifier, Duration), true);
        }

        // public override bool Apply(bool force = false){
        //     if(!base.Apply())
        //         return false;

        //     Target.aggro += aggroIncrease;
        //     return true;
        // }

        public override void Amplify(float modifier){
            aggroIncrease *= modifier;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }else{
                Target.aggro += aggroIncrease;
                return false;
            }
        }
    }
}