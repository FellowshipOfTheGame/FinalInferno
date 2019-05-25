using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class TriggerDialogue : Triggerable
    {
        [SerializeField] private Fog.Dialogue.Dialogue dialogue;
        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            if(dialogue != null){
                Fog.Dialogue.DialogueHandler.instance.StartDialogue(dialogue, agent, agent.GetComponent<Movable>());
            }
        }
    }
}
