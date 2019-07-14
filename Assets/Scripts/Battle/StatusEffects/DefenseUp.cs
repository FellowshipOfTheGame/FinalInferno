using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DefenseUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;
        private float valueReceived;

        public DefenseUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            defValue = Mathf.FloorToInt(trgt.curDef * value);
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DefenseUp(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Remove();
            defValue = Mathf.FloorToInt(modifier * defValue);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDef += defValue;
            return true;
        }

        public override void Remove() {
            Target.curDef -= defValue;
        }
    }
}