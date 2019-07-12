using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DamagingOverTime : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return dmgPerTurn; } }
        private int dmgPerTurn;
        private Element element;

        public DamagingOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur = 1) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            dmgPerTurn = Mathf.FloorToInt(Source.curDmg * value);
            Failed = !Apply();
        }

        public override void Amplify(float modifier){
            dmgPerTurn = Mathf.FloorToInt(modifier * dmgPerTurn);
        }

        public override bool Update(){
            Target.TakeDamage(dmgPerTurn, 1.0f, DamageType.None, element);
            return base.Update();
        }
    }
}