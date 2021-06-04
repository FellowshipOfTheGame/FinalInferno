using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LosingHPOverTime : StatusEffect {
        public override StatusEffectVisuals VFXID { get => StatusEffectVisuals.LosingHPOverTime; }
        public override StatusType Type { get{ return StatusType.Undesirable; } }
        public override float Value { get{ return Target.MaxHP * percentageLoss; } }
        private float percentageLoss;
        private Element element;

        public LosingHPOverTime(BattleUnit src, BattleUnit trgt, float value, Element elemnt, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            element = elemnt;
            percentageLoss = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new LosingHPOverTime(Source, target, percentageLoss * modifier, element, Duration), true);
        }

        public override void Amplify(float modifier){
            percentageLoss *= modifier;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }else{
                Target.TakeDamage(Mathf.Max(Mathf.FloorToInt(Target.MaxHP * percentageLoss), 1), 1.0f, DamageType.None, element, Source);
                return false;
            }
        }
    }
}