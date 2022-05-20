using System.Collections;
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
                StatusEffectVFX vfx = child.GetComponent<StatusEffectVFX>();
                if (vfx != null) {
                    statusEffectVFXes.Add(vfx);
                }
            }
        }

        public void Add(StatusEffect effect) {
            effects.Add(effect);
            turnsLeftMax = Mathf.Max(turnsLeftMax, effect.TurnsLeft);
            turnsLeftMin = Mathf.Min(turnsLeftMin, effect.TurnsLeft);
            Debug.Log($"Adding effect {effect.GetType().Name}");

            foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                vfx.ApplyTrigger();
            }
        }

        private int GetTurnsLeft(StatusEffectVFX vfx) {
            // Por precaução coloca o valor default para o de maior duração
            int turnsLeft = turnsLeftMax;
            switch (vfx.VisualBehaviour) {
                case StatusEffectVFX.TurnBehaviour.ShowLongest:
                    turnsLeft = turnsLeftMax;
                    break;
                case StatusEffectVFX.TurnBehaviour.ShowShortest:
                    turnsLeft = turnsLeftMin;
                    break;
                case StatusEffectVFX.TurnBehaviour.ShowNewest:
                    turnsLeft = effects[effects.Count - 1].TurnsLeft;
                    break;
                case StatusEffectVFX.TurnBehaviour.ShowOldest:
                    turnsLeft = effects[0].TurnsLeft;
                    break;
            }
            return turnsLeft;
        }

        public void Remove(StatusEffect effect) {
            effects.Remove(effect);
            turnsLeftMax = int.MinValue;
            turnsLeftMin = int.MaxValue;

            if (effects.Count <= 0) {
                // Caso seja o ultimo efeito, manda o trigger de animação para remover
                foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                    vfx.RemoveTrigger();
                }
            } else {
                // Caso não seja, avalia os novos valores de min/max
                foreach (StatusEffect eff in effects) {
                    turnsLeftMax = Mathf.Max(turnsLeftMax, eff.TurnsLeft);
                    turnsLeftMin = Mathf.Min(turnsLeftMin, eff.TurnsLeft);
                }

                // Atualiza o valor de turnsLeft de acordo com o comportamento do vfx
                foreach (StatusEffectVFX vfx in statusEffectVFXes) {
                    int turnsLeft = GetTurnsLeft(vfx);

                    // Se o novo valor for igual, não faz nada
                    if (turnsLeft != vfx.TurnsLeft) {
                        // Se tiver mudado, muda para o estado de idle correto
                        vfx.ResetTrigger();
                        vfx.TurnsLeft = turnsLeft;
                    }
                }
            }
        }

        public void ApplyChanges() {
            if (triggeredUpdate) {
                // Atualiza o valor de turnsLeft de acordo com o comportamento do vfx
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
    }
}
