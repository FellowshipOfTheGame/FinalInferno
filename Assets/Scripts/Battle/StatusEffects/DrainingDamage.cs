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

        public DrainingDamage(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false, bool dbleEdged = false, bool isPermnt = false) {
            // src é quem drena, trgt é quem é drenado
            // Isso conta como um buff que trgt aplica em src mesmo que src cause a aplicação do buff
            // Target então dever ser src e Source deve ser trgt
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = src;
            Source = trgt;
            doubleEdged = dbleEdged;
            multiplier = value;
            isPermanent = isPermnt;
            dmgValue = Mathf.Max(Mathf.FloorToInt(Source.curDmg * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new DrainingDamage(target, Source, multiplier * modifier, Duration), true);
        }

        public override void Amplify(float modifier){
            Target.curDmg -= dmgValue;
            dmgValue = Mathf.Max(Mathf.FloorToInt(modifier * dmgValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.curDmg += dmgValue;
            return true;
        }

        public override void Remove() {
            if(Source.CurHP > 0 || !isPermanent){
                Target.curDmg -= dmgValue;
                if(doubleEdged && Source.CurHP > 0){
                    // Remove o debuff que foi aplicado quando começou a drenar o dano, caso ainda exista
                    DamageDrained myDebuff = (DamageDrained)Source.effects.Find(debuff => (debuff.GetType() == typeof(DamageDrained) && debuff.Source == Target && debuff.Target == Source));
                    if(myDebuff != null) myDebuff.Remove();

                    // Aplica o dreno de dano ao contrario
                    Source.AddEffect(new DrainingDamage(Source, Target, multiplier, Duration));
                    Target.AddEffect(new DamageDrained(Source, Target, multiplier, Duration));
                }
            }
            base.Remove();
        }

        public override void ForceRemove(){
            Target.curDmg -= dmgValue;
            base.Remove();
        }
    }
}