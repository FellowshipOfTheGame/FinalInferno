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
                Quest actualQuest = StaticReferences.instance.activeQuests.Find(x => x.name == _event.quest.name);
                if(actualQuest == null)
                    actualQuest = _event.quest;

                if(actualQuest != null){
                    actualQuest.events[_event.eventFlag] = true;
                    Debug.Log("Dialogo triggerou evento " + _event.eventFlag + " da quest " + actualQuest);
                }
            }
        }
    }
}
