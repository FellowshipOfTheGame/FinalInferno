using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewEventDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/EventDialogue")]
    public class DialogueEventTrigger : DialogueFI {
        public QuestEvent[] eventsTriggered;

        public override void AfterDialogue() {
            TriggerEvents();
            base.AfterDialogue();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }

        private void TriggerEvents() {
            foreach (QuestEvent _event in eventsTriggered) {
                Quest quest = _event.quest;
                if (quest)
                    quest.SetFlag(_event.eventFlag, true);
            }
        }
    }
}
