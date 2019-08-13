using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class StatusResistUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return resistIncrease; } }
        private float resistIncrease;

        public StatusResistUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            resistIncrease = value;
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new StatusResistUp(Source, target, resistIncrease * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Remove();
            resistIncrease *= modifier;
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.statusResistance += resistIncrease;
            return true;
        }

        public override void Remove() {
            Target.statusResistance -= resistIncrease;
            base.Remove();
        }
    }
}