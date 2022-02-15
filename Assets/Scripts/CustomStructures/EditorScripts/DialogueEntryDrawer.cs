using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar DialogueEntry no editor da unity
    [CustomPropertyDrawer(typeof(DialogueEntry))]
    public class DialogueEntryDrawer : PropertyDrawer {

        private SerializedProperty quest, eventFlag, dialogue;
        private int index;
        private Rect questRect;
        private Rect eventRect;
        private Rect dialogueRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null) ? 2 : (_quest.objectReferenceValue == null) ? 2 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            DrawQuestField(position);
            DrawEventFlagFieldIfNecessary();
            DrawDialogueField();
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            dialogue = property.FindPropertyRelative("dialogue");
        }

        private void DrawQuestField(Rect position) {
            questRect = new Rect(position);
            questRect.y += 5f;
            questRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(questRect, quest);
        }

        private void DrawEventFlagFieldIfNecessary() {
            eventRect = (quest.objectReferenceValue == null) ? questRect : NewRectBelow(questRect);
            if (quest.objectReferenceValue != null) {
                DrawEventFlagField();
            } else {
                eventFlag.stringValue = "";
            }
        }

        private Rect NewRectBelow(Rect rect) {
            Rect returnValue = new Rect(rect);
            returnValue.y += rect.height;
            returnValue.height = EditorGUIUtility.singleLineHeight;
            return returnValue;
        }

        private void DrawEventFlagField() {
            Quest _quest = quest.objectReferenceValue as Quest;
            string[] keys = _quest.FlagNames;
            int indexOfSerializedFlag = System.Array.IndexOf(keys, eventFlag.stringValue);
            index = Mathf.Clamp(indexOfSerializedFlag, 0, Mathf.Max(keys.Length - 1, 0));
            index = EditorGUI.Popup(eventRect, "Event", index, keys);
            eventFlag.stringValue = (keys.Length > 0) ? keys[index] : "";
        }

        private void DrawDialogueField() {
            dialogueRect = NewRectBelow(eventRect);
            EditorGUI.PropertyField(dialogueRect, dialogue);
        }
    }
#endif
}