using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Regenerating : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return Target.MaxHP * percentageGain; } }
        private float percentageGain;
        private Element element;

        public Regenerating(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            percentageGain = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new Regenerating(Source, target, percentageGain * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            percentageGain *= modifier;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }else{
                Target.Heal(Mathf.Max(Mathf.FloorToInt(Target.MaxHP * percentageGain), 1), 1.0f, Source);
                return false;
            }
        }
    }
}