using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DamagingOverTime : StatusEffect {
        public override StatusType Type { get{ return StatusType.Undesirable; } }
        public override float Value { get{ return dmgPerTurn; } }
        private int dmgPerTurn;
        private float valueReceived;
        private Element element;

        public DamagingOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            valueReceived = value;
            dmgPerTurn = Mathf.Max(Mathf.FloorToInt(Source.curDmg * value), 1);
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DamagingOverTime(Source, target, valueReceived * modifier, element, Duration), true);
        }

        public override void Amplify(float modifier){
            dmgPerTurn = Mathf.Max(Mathf.FloorToInt(modifier * dmgPerTurn), 1);
        }

        public override bool Update(){
            Target.TakeDamage(dmgPerTurn, 1.0f, DamageType.None, element);
            return base.Update();
        }
    }
}