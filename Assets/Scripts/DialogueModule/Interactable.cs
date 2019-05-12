using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

namespace Fog.Dialogue
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour, IInteractable
    {
        public List<DialogueEntry> dialogues;

        public void Reset(){
            int nColliders = GetComponents<Collider2D>().Length;
            // Se so tem um collider, se certifica que ele seja trigger
            if(nColliders == 1)
                GetComponent<Collider2D>().isTrigger = true;
            else{
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

        public void Start(){
        }

        public void OnInteractAttempt(Agent agent, FinalInferno.Movable movingAgent = null){
            Fog.Dialogue.Dialogue selectedDialogue = null;

            foreach(DialogueEntry entry in dialogues){
                if(entry.quest.events[entry.eventFlag])
                    selectedDialogue = entry.dialogue;
                else
                    break;
            }
            if(selectedDialogue != null){
                Fog.Dialogue.DialogueHandler.instance.dialogue = selectedDialogue;
                Fog.Dialogue.DialogueHandler.instance.StartDialogue(agent, movingAgent);
            }
        }
    }
}
