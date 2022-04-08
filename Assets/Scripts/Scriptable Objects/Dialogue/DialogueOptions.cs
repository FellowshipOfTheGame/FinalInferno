using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewOptionsDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueOptions")]
    public class DialogueOptions : Fog.Dialogue.OptionsDialogue {
        public override void BeforeDialogue() {
            CharacterOW.PartyCanMove = false;
            base.BeforeDialogue();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }
    }
}