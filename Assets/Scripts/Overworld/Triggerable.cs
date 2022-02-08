using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D))]
    public abstract class Triggerable : MonoBehaviour {
        public void Reset() {
            int nColliders = GetComponents<Collider2D>().Length;
            // Se so tem um collider, se certifica que ele seja trigger
            if (nColliders == 1) {
                GetComponent<Collider2D>().isTrigger = true;
            } else {
                bool hasTrigger = false;
                // Se tiver mais de um collider, verifica se ao menos um deles e trigger
                foreach (Collider2D col in GetComponents<Collider2D>()) {
                    hasTrigger = col.isTrigger;
                    if (hasTrigger) {
                        break;
                    }
                }
                // Se nenhum deles for, se certifica de que o primeiro deles seja trigger
                if (!hasTrigger) {
                    GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }

        protected abstract void TriggerAction(Fog.Dialogue.Agent agent);

        private void OnTriggerEnter2D(Collider2D col) {
            Fog.Dialogue.Agent agent = col.GetComponent<Fog.Dialogue.Agent>();
            if (agent != null) {
                TriggerAction(agent);
            }
        }
    }
}
