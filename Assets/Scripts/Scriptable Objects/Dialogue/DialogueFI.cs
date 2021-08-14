using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/Dialogue")]
    public class DialogueFI : Fog.Dialogue.Dialogue {
        protected bool shouldUnlockMovement = true;

        public override void BeforeDialogue(){
            CharacterOW.PartyCanMove = false;

            base.BeforeDialogue();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }

        public override void AfterDialogue(){
            if(shouldUnlockMovement)
                CharacterOW.PartyCanMove = true;

            base.AfterDialogue();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }
    }
}