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
            Apply();
        }

        public override void Amplify(float modifier){
            Remove();
            resistIncrease *= modifier;
            Apply();
        }

        public override void Apply() {
            Target.statusResistance += resistIncrease;
        }

        public override void Remove() {
            Target.statusResistance -= resistIncrease;
        }
    }
}