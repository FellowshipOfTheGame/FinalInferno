using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DamageDrained : StatusEffect {
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return dmgValue; } }
        private int dmgValue;

        public DamageDrained(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            dmgValue = Mathf.FloorToInt(trgt.curDmg * value);
            Apply();
        }

        public override void Amplify(float modifier){
            dmgValue = Mathf.FloorToInt(modifier * dmgValue);
            Remove();
            Apply();
        }

        public override void Apply() {
            Target.curDmg -= dmgValue;
        }

        public override void Remove() {
            Target.curDmg += dmgValue;
        }
    }
}