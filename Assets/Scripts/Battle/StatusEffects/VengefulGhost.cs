using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public class VengefulGhost : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.VengefulGhost;
        public override StatusType Type => StatusType.None;
        public override float Value => Duration;
        private float dmgModifier;
        private int dmgChange;

        public VengefulGhost(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;

            Duration = dur;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            dmgModifier = value;
            Failed = Target.CurHP > 0 || !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new VengefulGhost(Source, target, dmgModifier * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force))
                return false;

            Target.stuns++;
            dmgChange = Mathf.FloorToInt((dmgModifier * Target.CurDmg) - Target.CurDmg);
            Target.CurDmg += dmgChange;
            Target.Ghost = true;
            return true;
        }

        public override void Amplify(float modifier) {
            dmgModifier *= modifier;
            Target.CurDmg -= dmgChange;
            dmgChange = Mathf.FloorToInt((dmgModifier * Target.CurDmg) - Target.CurDmg);
            Target.CurDmg += dmgChange;
        }

        public override bool Update() {
            if (Target.CurHP > 0) {
                Remove();
                return true;
            }
            if (base.Update())
                return true;


            if (IsFirstEffectInTargetList(typeof(VengefulGhost)))
                AttackHighestAggroEnemy();

            return false;
        }

        private void AttackHighestAggroEnemy() {
            Target.SkillSelected();
            List<BattleUnit> enemies = BattleManager.instance.GetEnemies(Target);
            BattleUnit selectedEnemy = FindHighestAggroEnemy(enemies);
            enemies.Clear();
            enemies.Add(selectedEnemy);
            Target.Unit.attackSkill.UseCallbackOrDelayed(Target, enemies);
        }

        private static BattleUnit FindHighestAggroEnemy(List<BattleUnit> enemies) {
            int selected = 0;
            for (int i = 1; i < enemies.Count; i++) {
                if (enemies[i].aggro > enemies[selected].aggro)
                    selected = i;
            }
            return enemies[selected];
        }

        public override void Remove() {
            Target.stuns--;
            Target.CurDmg -= dmgChange;
            Target.Ghost = false;
            base.Remove();
        }
    }
}