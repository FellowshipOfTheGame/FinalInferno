using System.Collections.Generic;
using Fog.Dialogue;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour, IInteractable {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();

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
                if (col.isTrigger) {
                    return true;
                }
            }
            return false;
        }

        public void OnInteractAttempt() {
            Dialogue selectedDialogue = DialogueEntry.GetLastUnlockedDialogue(dialogues);
            if (selectedDialogue != null) {
                DialogueHandler.instance.StartDialogue(selectedDialogue);
            }
        }

        public void OnTriggerEnter2D(Collider2D col) {
            Agent agent = col.GetComponent<Agent>();
            if (agent)
                agent.collidingInteractables.Add(this);
        }

        public void OnTriggerExit2D(Collider2D col) {
            Agent agent = col.GetComponent<Agent>();
            if (agent)
                agent.collidingInteractables.Remove(this);
        }

    }
}
