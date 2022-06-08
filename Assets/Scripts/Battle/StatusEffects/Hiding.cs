﻿using UnityEngine;

namespace FinalInferno {
    public class Hiding : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.Hiding;
        public override StatusType Type => StatusType.None;
        public override float Value => negativeAggro;
        private int turnsLeft;
        private float negativeAggro;

        public Hiding(BattleUnit src, BattleUnit trgt, float value, int dur, bool force = false) {
            if (dur < 0)
                dur = int.MinValue;
            // Como o efeito é aplicado pela primeira vez no apply ao inves do primeiro update
            // o valor de Duration precisa ser decrementado imediatamente
            Duration = (dur == int.MinValue) ? dur : dur - 1;
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            negativeAggro = value;
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new Hiding(Source, target, negativeAggro * modifier, Duration), true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply())
                return false;

            Target.aggro = -negativeAggro;
            return true;
        }

        public override void Amplify(float modifier) {
            negativeAggro *= modifier;
        }

        public override bool Update() {
            if (base.Update())
                return true;

            Target.aggro = -negativeAggro;
            return false;
        }

        public override void Remove() {
            Target.aggro = Mathf.Max(0f, Target.aggro);
            base.Remove();
        }
    }
}