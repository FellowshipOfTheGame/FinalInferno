using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Malee;

namespace Fog.Dialogue
{
    /// <summary>
    ///     Creates a scriptable object for an array of dialogue lines, so that it can be saved as a file.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObject/DialogueSystem/Dialogue")]
    public class Dialogue : ScriptableObject
    {
#if UNITY_EDITOR
        protected static ReorderableDialogueList clipboard = null;

        [ContextMenu("Copy")]
        void CopyLines(){
            // clipboard = lines.Clone() as ReorderableDialogueList;
            clipboard = new ReorderableDialogueList();
            foreach(DialogueLine line in lines){
                clipboard.Add(line.Clone());
            }
        }

        [ContextMenu("Paste")]
        void PasteLines(){
            if(clipboard != null){
                // lines = clipboard.Clone() as ReorderableDialogueList;
                UnityEditor.Undo.RecordObject(this, "Pasted Dialogue Lines");
                lines = new ReorderableDialogueList();
                foreach(DialogueLine line in clipboard){
                    lines.Add(line.Clone());
                }
            }
        }
#endif
        [Reorderable] public ReorderableDialogueList lines;

        public virtual void BeforeDialogue(){
			if(Agent.Instance){
				Agent.Instance.canInteract = false;
			}
            DialogueHandler.instance.OnDialogueStart -= BeforeDialogue;
        }

        public virtual void AfterDialogue(){
			if(Agent.Instance){
				// Input cooldown is needed because it uses the same "Interactable" button
				Agent.Instance.InputCooldown();
				Agent.Instance.canInteract = true;
			}
            DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }
    }

    [System.Serializable]
    public class ReorderableDialogueList : ReorderableArray<DialogueLine> {}
}