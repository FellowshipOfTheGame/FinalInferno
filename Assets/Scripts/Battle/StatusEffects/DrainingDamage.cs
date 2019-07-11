using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class DrainingDamage : StatusEffect {
        public override StatusType Type { get{ return StatusType.Buff; } }
        public override float Value { get{ return dmgValue; } }
        private int dmgValue;
        private float multiplier;
        private bool doubleEdged;
        private bool isPermanent;

        public DrainingDamage(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool dbleEdged = false, bool isPermnt = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            doubleEdged = dbleEdged;
            multiplier = value;
            isPermanent = isPermnt;
            dmgValue = Mathf.FloorToInt(trgt.curDmg * value);
            Apply();
        }

        public override void Amplify(float modifier){
            Source.curDmg -= dmgValue; // Se chamar Remove pode dar problema
            dmgValue = Mathf.FloorToInt(modifier * dmgValue);
            Apply();
        }

        public override void Apply() {
            Source.curDmg += dmgValue;
        }

        public override void Remove() {
            if(Target.CurHP > 0 || !isPermanent){
                Source.curDmg -= dmgValue;
                if(doubleEdged){
                    Target.AddEffect(new DrainingDamage(Target, Source, multiplier, Duration));
                    Source.AddEffect(new DamageDrained(Target, Source, multiplier, Duration));
                }
            }
        }
    }
}