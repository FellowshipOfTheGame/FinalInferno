using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DefenseDown : StatusEffect {
        public override StatusEffectVisuals VFXID { get => StatusEffectVisuals.DefenseDown; }
        public override StatusType Type { get{ return StatusType.Debuff; } }
        public override float Value { get{ return defValue; } }
        private int defValue;
        private float valueReceived;

        public DefenseDown(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            valueReceived = value;
            defValue = Mathf.Max(Mathf.FloorToInt(trgt.curDef * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DefenseDown(Source, target, valueReceived * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Target.curDef += defValue;
            defValue = Mathf.Max(Mathf.FloorToInt(modifier * defValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDef -= defValue;
            return true;
        }

        public override void Remove() {
            Target.curDef += defValue;
            base.Remove();
        }
    }
}