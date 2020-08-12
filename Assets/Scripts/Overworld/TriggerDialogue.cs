using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class TriggerDialogue : Triggerable
    {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            Fog.Dialogue.Dialogue selectedDialogue = null;
            foreach(DialogueEntry entry in dialogues){
                if(entry.quest != null && entry.quest.events[entry.eventFlag]){
                    selectedDialogue = entry.dialogue;
                }else
                    break;
            }

            if(selectedDialogue != null){
                Fog.Dialogue.DialogueHandler.instance.StartDialogue(selectedDialogue);
            }
        }
    }
}
