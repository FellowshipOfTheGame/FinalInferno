using System.Collections.Generic;
using UnityEngine;
using Fog.Dialogue;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D))]
    public abstract class Triggerable : MonoBehaviour {
        public void Reset() {
            Collider2D[] colliders = GetComponents<Collider2D>();
            EnsureAtLeastOneTrigger(colliders);
        }

        private void EnsureAtLeastOneTrigger(Collider2D[] colliders) {
            int nColliders = colliders.Length;
            if (nColliders == 1) {
                colliders[0].isTrigger = true;
            } else if (nColliders > 0) {
                bool alreadyHasTrigger = HasAtLeastOneTrigger(colliders);
                if (!alreadyHasTrigger) {
                    colliders[0].isTrigger = true;
                }
            }
        }

        private bool HasAtLeastOneTrigger(Collider2D[] colliders) {
            foreach (Collider2D col in colliders) {
                if(col.isTrigger) {
                    return true;
                }
            }
            return false;
        }

        protected abstract void TriggerAction(Agent agent);

        private void OnTriggerEnter2D(Collider2D col) {
            Agent agent = col.GetComponent<Agent>();
            if (agent != null) {
                TriggerAction(agent);
            }
        }
    }
}
