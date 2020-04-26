using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class HealingOverTime : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return healPerTurn; } }
        private int healPerTurn;
        private float valueReceived;

        public HealingOverTime(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            healPerTurn = Mathf.Max(Mathf.FloorToInt(Source.curDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new HealingOverTime(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            healPerTurn = Mathf.Max(Mathf.FloorToInt(modifier * healPerTurn), 1);
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }else{
                Target.Heal(healPerTurn, 1.0f, Source);
                return false;
            }
        }
    }
}