using System.Collections.Generic;
using Fog.Dialogue;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour, IInteractable {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        public void Reset() {
            int nColliders = GetComponents<Collider2D>().Length;
            // Se so tem um collider, se certifica que ele seja trigger
            if (nColliders == 1) {
                GetComponent<Collider2D>().isTrigger = true;
            } else if (nColliders > 0) {
                bool hasTrigger = HasAtLeastOneTrigger();
                // Se nenhum deles for, se certifica de que o primeiro deles seja trigger
                if (!hasTrigger) {
                    GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }

        private bool HasAtLeastOneTrigger() {
            foreach (Collider2D col in GetComponents<Collider2D>()) {
                if(col.isTrigger) {
                    return true;
                }
            }
            return false;
        }

        public void OnInteractAttempt() {
            Dialogue selectedDialogue = null;
            foreach (DialogueEntry entry in dialogues) {
                if (entry.quest != null && entry.quest.GetFlag(entry.eventFlag)) {
                    selectedDialogue = entry.dialogue;
                } else {
                    break;
                }
            }
            if (selectedDialogue != null) {
                DialogueHandler.instance.StartDialogue(selectedDialogue);
            }
        }

        public void OnTriggerEnter2D(Collider2D col) {
            Agent agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Add(this);
            }
        }

        public void OnTriggerExit2D(Collider2D col) {
            Agent agent = col.GetComponent<Agent>();
            if (agent) {
                agent.collidingInteractables.Remove(this);
            }
        }

    }
}
