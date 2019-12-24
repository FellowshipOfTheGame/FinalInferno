using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewEventDialogue", menuName = "ScriptableObject/DialogueSystem/EventDialogue")]
    public class DialogueEventTrigger : Fog.Dialogue.Dialogue
    {
        public QuestEvent[] eventsTriggered;

        public override void AfterDialogue(){
            foreach(QuestEvent _event in eventsTriggered){
                Quest quest = _event.quest.PartyReference;
                if(quest == null)
                    quest = _event.quest;

                if(quest != null){
                    quest.events[_event.eventFlag] = true;
                    Debug.Log("Dialogo triggerou evento " + _event.eventFlag + " da quest " + quest);
                }
            }
        }
    }
}
