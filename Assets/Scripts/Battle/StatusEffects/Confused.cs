﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class Confused : StatusEffect {
        public override StatusEffectVisuals VFXID { get => StatusEffectVisuals.Confused; }
        public override StatusType Type { get{ return StatusType.Undesirable; } }
        public override float Value { get{ return Duration; } }

        public Confused(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            rollValue = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new Confused(Source, target, rollValue * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.stuns++;
            return true;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }

            // Apenas um dos status effects desse tipo causa um ataque
            if(Target.effects.Find(effect => effect.GetType() == typeof(Confused)) == this){
                Target.SkillSelected();

                List<BattleUnit> allies = BattleManager.instance.GetTeam(Target);
                int selected = Random.Range(0, allies.Count);
                List<BattleUnit> enemy = new List<BattleUnit>();
                enemy.Add(allies[selected]);

                Target.Unit.attackSkill.Use(Target, enemy);
            }

            return false;
        }

        public override void Remove() {
            Target.stuns--;
            base.Remove();
        }
    }
}