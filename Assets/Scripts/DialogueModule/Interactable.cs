using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

namespace Fog.Dialogue
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();

        void Awake(){
            for(int i = 0; i < dialogues.Count; i++){
                if(dialogues[i].quest.StaticReference != null){
                    dialogues[i] = new DialogueEntry(dialogues[i].quest.StaticReference, dialogues[i].eventFlag, dialogues[i].dialogue);
                }
            }
        }

        public void Reset(){
            int nColliders = GetComponents<Collider2D>().Length;
            // Se so tem um collider, se certifica que ele seja trigger
            if(nColliders == 1){
                GetComponent<Collider2D>().isTrigger = true;
            }else{
                bool hasTrigger = false;
                // Se tiver mais de um collider, verifica se ao menos um deles e trigger
                foreach(Collider2D col in GetComponents<Collider2D>()){
                    hasTrigger = col.isTrigger;
                    if(hasTrigger)
                        break;
                }
                // Se nenhum deles for, se certifica de que o primeiro deles seja trigger
                if(!hasTrigger)
                    GetComponent<Collider2D>().isTrigger = true;
            }
        }

        public void OnInteractAttempt(Agent agent, FinalInferno.Movable movingAgent = null){
            Dialogue selectedDialogue = null;
            foreach(DialogueEntry entry in dialogues){
                if(entry.quest != null && entry.quest.events[entry.eventFlag]){
                    selectedDialogue = entry.dialogue;
                }else
                    break;
            }
            if(selectedDialogue != null){
                Fog.Dialogue.DialogueHandler.instance.StartDialogue(selectedDialogue, agent, movingAgent);
            }
        }

        public void OnTriggerEnter2D(Collider2D col){
            Agent agent = col.GetComponent<Agent>();
            if(agent){
                agent.collidingInteractables.Add(this);
            }
        }

        public void OnTriggerExit2D(Collider2D col){
            Agent agent = col.GetComponent<Agent>();
            if(agent){
                agent.collidingInteractables.Remove(this);
            }
        }

    }
}
