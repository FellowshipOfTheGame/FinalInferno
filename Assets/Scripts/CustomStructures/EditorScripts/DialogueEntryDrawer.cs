using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar DialogueEntry no editor da unity
    [CustomPropertyDrawer(typeof(DialogueEntry))]
    public class DialogueEntryDrawer : PropertyDrawer {

        private SerializedProperty dialogue;
        private Rect dialogueRect;
        private QuestEventField questEventField = new QuestEventField();
        private const float marginSize = 5f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float questEventFieldHeight = questEventField.GetFieldHeight(property);
            return questEventFieldHeight + EditorGUIUtility.singleLineHeight + (2 * marginSize);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            position.y += marginSize;
            questEventField.DrawQuestEventField(position);
            DrawDialogueField();
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            questEventField.FindSerializedStructProperties(property);
            dialogue = property.FindPropertyRelative("dialogue");
        }

        private void DrawDialogueField() {
            dialogueRect = EditorUtils.NewRectBelow(questEventField.EventRect);
            EditorGUI.PropertyField(dialogueRect, dialogue);
        }
    }
#endif
}