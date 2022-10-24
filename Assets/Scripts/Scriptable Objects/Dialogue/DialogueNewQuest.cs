using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewStartQuestDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/NewQuestDialogue")]
    public class DialogueNewQuest : DialogueEventTrigger {
        [SerializeField] private Quest questToStart;

        public override void AfterDialogue() {
            base.AfterDialogue();
            if (!questToStart.IsActive) {
                Party.Instance.StartQuest(questToStart);
            } else {
                Debug.LogWarning($"Quest {questToStart} has already begun", this);
            }
            Fog.Dialogue.DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }
    }
}
