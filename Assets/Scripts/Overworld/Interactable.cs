using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

public class Interactable : MonoBehaviour, IInteractable
{
    public List<DialogueEntry> dialogues;

    public void OnInteractAttempt(){
        Fog.Dialogue.Dialogue selectedDialogue = null;
        foreach(DialogueEntry entry in dialogues){
            if(entry.quest.events[entry.eventFlag])
                selectedDialogue = entry.dialogue;
        }
        if(selectedDialogue != null){
            Fog.Dialogue.DialogueHandler.instance.dialogue = selectedDialogue;
            Fog.Dialogue.DialogueHandler.instance.StartDialogue();
        }
    }
}
