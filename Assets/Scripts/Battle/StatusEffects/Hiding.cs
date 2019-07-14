using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Hiding : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return negativeAggro; } }
        private int turnsLeft;
        public override int TurnsLeft { protected set{ base.TurnsLeft = value; } get{ return (base.TurnsLeft > int.MinValue)? base.TurnsLeft-1 : 99;} }
        private float negativeAggro;

        public Hiding(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            negativeAggro = value;
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new Hiding(Source, target, negativeAggro * modifier, Duration), true);
        }

        public override bool Apply(bool force = false){
            if(!base.Apply())
                return false;

            Target.aggro = -negativeAggro;
            return true;
        }

        public override void Amplify(float modifier){
            negativeAggro *= modifier;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }else{
                Target.aggro = -negativeAggro;
                return false;
            }
        }
    }
}