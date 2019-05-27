using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewEndQuestDialogue", menuName = "ScriptableObject/DialogueSystem/EndQuestDialogue")]
    public class DialogueEndQuest : DialogueEventTrigger
    {
        [SerializeField] private Quest questToEnd;

        public override void AfterDialogue(){
            base.AfterDialogue();
            questToEnd.CompleteQuest();
        }
    }
}
