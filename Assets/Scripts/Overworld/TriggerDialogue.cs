using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class TriggerDialogue : Triggerable
    {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        private Fog.Dialogue.Dialogue selectedDialogue = null;
        private int currentIndex = -1;

        void Awake(){
            for(int i = 0; i < dialogues.Count; i++){
                if(dialogues[i].quest.StaticReference != null){
                    dialogues[i] = new DialogueEntry(dialogues[i].quest.StaticReference, dialogues[i].eventFlag, dialogues[i].dialogue);
                }
            }
        }

        void Start(){
            for(int i = 0; i < dialogues.Count; i++){
                if(dialogues[i].quest.events[dialogues[i].eventFlag]){
                    selectedDialogue = dialogues[i].dialogue;
                    currentIndex = i;
                }else
                    break;
            }
        }

        void Update(){
            for(int i = currentIndex; i < dialogues.Count; i++){
                if(dialogues[i].quest.events[dialogues[i].eventFlag]){
                    selectedDialogue = dialogues[i].dialogue;
                    currentIndex = i;
                }else
                    break;
            }
        }

        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            Fog.Dialogue.Dialogue selectedDialogue = null;

            foreach(DialogueEntry entry in dialogues){
                if(entry.quest.events[entry.eventFlag])
                    selectedDialogue = entry.dialogue;
                else
                    break;
            }
            if(selectedDialogue != null){
                Fog.Dialogue.DialogueHandler.instance.StartDialogue(selectedDialogue, agent, agent.GetComponent<Movable>());
            }
        }
    }
}
