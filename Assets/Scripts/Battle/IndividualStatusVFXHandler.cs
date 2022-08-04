using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public class IndividualStatusVFXHandler {
        public List<StatusEffect> effects;
        public int turnsLeftMin;
        public int turnsLeftMax;
        public bool triggeredUpdate;
        private Transform transform;
        private List<StatusEffectVFX> statusEffectVFXes = new List<StatusEffectVFX>();

        public IndividualStatusVFXHandler(Transform t) {
            effects = new List<StatusEffect>();
            turnsLeftMax = int.MinValue;
            turnsLeftMin = int.MaxValue;
            triggeredUpdate = false;
            transform = t;
            foreach (Transform child in transform) {
                if (child.TryGetComponent(out StatusEffectVFX vfx))
                    statusEffectVFXes.Add(vfx);
            }
        }

        public void ApplyChanges() {
            if (triggeredUpdate) {
                foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                    int turnsLeft = GetTurnsLeft(vfx);
                    vfx.TurnsLeft = turnsLeft;
                    vfx.UpdateTrigger();
                }
            }
            turnsLeftMax = int.MinValue;
            turnsLeftMin = int.MaxValue;
            triggeredUpdate = false;
        }

        private int GetTurnsLeft(StatusEffectVFX vfx) {
            return vfx.VisualBehaviour switch {
                StatusEffectVFX.TurnBehaviour.ShowLongest => turnsLeftMax,
                StatusEffectVFX.TurnBehaviour.ShowShortest => turnsLeftMin,
                StatusEffectVFX.TurnBehaviour.ShowNewest => effects[effects.Count - 1].TurnsLeft,
                StatusEffectVFX.TurnBehaviour.ShowOldest => effects[0].TurnsLeft,
                _ => turnsLeftMax,
            };
        }

        public void Add(StatusEffect effect) {
            effects.Add(effect);
            turnsLeftMax = Mathf.Max(turnsLeftMax, effect.TurnsLeft);
            turnsLeftMin = Mathf.Min(turnsLeftMin, effect.TurnsLeft);
            foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                vfx.ApplyTrigger();
            }
        }

        public void Remove(StatusEffect effect) {
            effects.Remove(effect);
            turnsLeftMax = int.MinValue;
            turnsLeftMin = int.MaxValue;

            if (effects.Count <= 0) {
                foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                    vfx.RemoveTrigger();
                }
            } else {
                RecalculateMinMaxValues();
                UpdateTurnsLeftValue();
            }
        }

        private void RecalculateMinMaxValues() {
            foreach (StatusEffect eff in effects) {
                turnsLeftMax = Mathf.Max(turnsLeftMax, eff.TurnsLeft);
                turnsLeftMin = Mathf.Min(turnsLeftMin, eff.TurnsLeft);
            }
        }

        private void UpdateTurnsLeftValue() {
            foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                int turnsLeft = GetTurnsLeft(vfx);
                if (turnsLeft == vfx.TurnsLeft)
                    continue;
                vfx.ResetTrigger();
                vfx.TurnsLeft = turnsLeft;
            }
        }
    }
}
