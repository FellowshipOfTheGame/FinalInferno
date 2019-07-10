using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DefenseUp : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;

        public DefenseUp(BattleUnit src, BattleUnit trgt, float value, int dur = 1) {
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            defValue = Mathf.FloorToInt(trgt.curDef * value);
            Apply();
        }

        public override void Amplify(float modifier){
            defValue = Mathf.FloorToInt(modifier * defValue);
            Remove();
            Apply();
        }

        public override void Apply() {
            Target.curDef += defValue;
        }

        public override void Remove() {
            Target.curDef -= defValue;
        }
    }
}