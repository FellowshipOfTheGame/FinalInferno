using System.Collections.Generic;
using Fog.Dialogue;
using UnityEngine;

namespace FinalInferno {
    public class TriggerDialogue : Triggerable {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        protected override void TriggerAction(Agent agent) {
            Dialogue selectedDialogue = DialogueEntry.GetLastUnlockedDialogue(dialogues);
            if (selectedDialogue != null) {
                DialogueHandler.instance.StartDialogue(selectedDialogue);
            }
        }
    }
}
