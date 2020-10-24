using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class VengefulGhost : StatusEffect {
        public override StatusType Type { get{ return StatusType.None; } }
        public override float Value { get{ return Duration; } }
        private float dmgModifier;
        private int dmgChange;

        public VengefulGhost(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            if(dur < 0)
                dur = int.MinValue;
            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            dmgModifier = value;
            Failed = (Target.CurHP > 0 || !Apply(force));
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f){
            target.AddEffect(new VengefulGhost(Source, target, dmgModifier * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if(!base.Apply(force))
                return false;
            Target.stuns++;
            dmgChange = Mathf.FloorToInt((dmgModifier * Target.curDmg) - Target.curDmg);
            Target.curDmg += dmgChange;
            Target.Ghost = true;
            return true;
        }

        public override void Amplify(float modifier){
            dmgModifier *= modifier;
            Target.curDmg -= dmgChange;
            dmgChange = Mathf.FloorToInt((dmgModifier * Target.curDmg) - Target.curDmg);
            Target.curDmg += dmgChange;
        }

        public override bool Update(){
            if(base.Update()){
                return true;
            }

            if(Target.CurHP > 0){
                return true;
            }

            // Apenas um dos status effects desse tipo causa um ataque
            if(Target.effects.Find(effect => effect.GetType() == typeof(VengefulGhost)) == this){
                Target.SkillSelected();

                List<BattleUnit> enemies = BattleManager.instance.GetEnemies(Target);
                int selected = 0;
                // Sempre ataca a unidade com mais aggro
                for(int i = 1; i < enemies.Count; i++){
                    if(enemies[i].aggro > enemies[selected].aggro)
                        selected = i;
                }
                List<BattleUnit> enemy = new List<BattleUnit>();
                enemy.Add(enemies[selected]);

                Target.unit.attackSkill.Use(Target, enemy);
            }

            return false;
        }

        public override void Remove() {
            Target.stuns--;
            Target.curDmg -= dmgChange;
            Target.Ghost = false;
            base.Remove();
        }
    }
}