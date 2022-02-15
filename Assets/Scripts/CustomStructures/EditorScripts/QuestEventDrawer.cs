using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomPropertyDrawer(typeof(QuestEvent))]
    public class QuestEventDrawer : PropertyDrawer {
        private SerializedProperty quest, eventFlag;
        private int index;
        private Rect questRect;
        private Rect eventRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null) ? 1 : (_quest.objectReferenceValue == null) ? 1 : 2;
            return (i * EditorGUIUtility.singleLineHeight) + 5f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructureProperties(property);
            DrawQuestField(position);
            DrawEventFlagFieldIfNecessary();
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructureProperties(SerializedProperty property) {
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
        }

        private void DrawQuestField(Rect position) {
            questRect = new Rect(position);
            questRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(questRect, quest);
        }

        private void DrawEventFlagFieldIfNecessary() {
            if (quest.objectReferenceValue != null) {
                DrawEventFlagField();
            } else {
                eventFlag.stringValue = "";
            }
        }

        private void DrawEventFlagField() {
            eventRect = NewRectBelow(questRect);
            Quest _quest = quest.objectReferenceValue as Quest;
            string[] keys = _quest.FlagNames;
            int indexOfSerializedFlag = System.Array.IndexOf(keys, eventFlag.stringValue);
            index = Mathf.Clamp(indexOfSerializedFlag, 0, Mathf.Max(keys.Length - 1, 0));
            index = EditorGUI.Popup(eventRect, "Event", index, keys);
            eventFlag.stringValue = (keys.Length > 0) ? keys[index] : "";
        }

        private Rect NewRectBelow(Rect rect) {
            Rect returnValue = new Rect(rect);
            returnValue.y += rect.height;
            returnValue.height = EditorGUIUtility.singleLineHeight;
            return returnValue;
        }
    }
#endif
}