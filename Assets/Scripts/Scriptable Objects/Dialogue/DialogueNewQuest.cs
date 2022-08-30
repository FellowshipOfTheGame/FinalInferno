﻿using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewStartQuestDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/NewQuestDialogue")]
    public class DialogueNewQuest : DialogueEventTrigger {
        [SerializeField] private Quest questToStart;

        public override void AfterDialogue() {
            base.AfterDialogue();
            questToStart.TryStartQuest();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }
    }
}
