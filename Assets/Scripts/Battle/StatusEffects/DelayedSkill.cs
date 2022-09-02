using System.Collections.Generic;

namespace FinalInferno {
    public class DelayedSkill : StatusEffect {
        public override StatusEffectVisuals VFXID => StatusEffectVisuals.DelayedSkill;
        private SkillDelegate callback = null;
        public override StatusType Type => StatusType.None;
        public override float Value => value1;
        private List<BattleUnit> targetList = new List<BattleUnit>();
        private bool override1 = false;
        private bool override2 = false;
        private float value1 = 0f;
        private float value2 = 0f;

        public DelayedSkill(SkillDelegate cb, BattleUnit src, BattleUnit trgt, bool shouldOverride1 = false, float v1 = 0f, bool shouldOverride2 = false, float v2 = 0f, int dur = 1, bool force = true) {
            if (dur < 0)
                dur = 1;

            Duration = dur - 1; // Necessario para que a contagem de delay seja intuitiva
            TurnsLeft = Duration;
            Target = trgt;
            Source = src;
            targetList.Add(trgt);
            override1 = shouldOverride1;
            override2 = shouldOverride2;
            value1 = v1;
            value2 = v2;
            callback += cb;
            Failed = !Apply(force);
        }

        public override void Remove() {
            if (TurnsLeft < 0)
                callback?.Invoke(Source, targetList, override1, value1, override2, value2);
            base.Remove();
        }

        public override void ForceRemove() {
            base.Remove();
        }
    }
}