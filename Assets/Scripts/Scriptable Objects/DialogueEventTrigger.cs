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
                if(_event.quest){
                    _event.quest.events[_event.eventFlag] = true;
                }
            }
        }
    }
}
