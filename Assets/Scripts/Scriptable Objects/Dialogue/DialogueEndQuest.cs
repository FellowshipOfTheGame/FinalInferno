using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewEndQuestDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/EndQuestDialogue")]
    public class DialogueEndQuest : DialogueEventTrigger {
        [SerializeField] private Quest questToEnd;

        public override void AfterDialogue() {
            base.AfterDialogue();
            Party.Instance.CompleteQuest(questToEnd);
            Fog.Dialogue.DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }
    }
}
