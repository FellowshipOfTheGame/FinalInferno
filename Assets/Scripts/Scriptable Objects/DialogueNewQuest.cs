using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewStartQuestDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/NewQuestDialogue")]
    public class DialogueNewQuest : DialogueEventTrigger
    {
        [SerializeField] private Quest questToStart;

        public override void AfterDialogue(){
            base.AfterDialogue();
            questToStart.StartQuest();
        }
    }
}
