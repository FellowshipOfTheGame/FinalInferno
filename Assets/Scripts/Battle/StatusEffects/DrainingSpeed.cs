using UnityEngine;

namespace FinalInferno {
    public class DrainingSpeed : StatusEffect {
        public override StatusType Type => StatusType.Buff;
        public override float Value => spdValue;
        private int spdValue;
        private float multiplier;

        public DrainingSpeed(BattleUnit src, BattleUnit trgt, float value, int dur = 1, bool force = false) {
            // src é quem drena, trgt é quem é drenado
            // Isso conta como um buff que trgt aplica em src mesmo que src cause a aplicação do buff
            // Target então dever ser src e Source deve ser trgt
            if (dur < 0) {
                dur = int.MinValue;
            }

            Duration = dur;
            TurnsLeft = Duration;
            Target = src;
            Source = trgt;
            multiplier = value;
            spdValue = Mathf.Max(Mathf.FloorToInt(Source.curSpeed * value), 1);
            Failed = !Apply(force);
        }

        public override void CopyTo(BattleUnit target, float modifier = 1.0f) {
            target.AddEffect(new DrainingSpeed(target, Source, multiplier * modifier, Duration), true);
        }

        public override void Amplify(float modifier) {
            Target.curSpeed -= spdValue;
            spdValue = Mathf.Max(Mathf.FloorToInt(modifier * spdValue), 1);
            Apply(true);
        }

        public override bool Apply(bool force = false) {
            if (!base.Apply(force)) {
                return false;
            }

            Target.curSpeed += spdValue;
            return true;
        }

        public override void Remove() {
            Target.curSpeed -= spdValue;
            base.Remove();
        }
    }
}