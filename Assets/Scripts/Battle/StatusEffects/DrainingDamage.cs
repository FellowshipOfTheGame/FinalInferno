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
            Failed = !Apply();
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DrainingDamage(Source, target, multiplier * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Source.curDmg -= dmgValue; // Se chamar Remove pode dar problema
            dmgValue = Mathf.FloorToInt(modifier * dmgValue);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Source.curDmg += dmgValue;
            return true;
        }

        public override void Remove() {
            if(Target.CurHP > 0 || !isPermanent){
                Source.curDmg -= dmgValue;
                if(doubleEdged){
                    Target.AddEffect(new DrainingDamage(Target, Source, multiplier, Duration));
                    Source.AddEffect(new DamageDrained(Target, Source, multiplier, Duration));
                }
            }
            base.Remove();
        }
    }
}