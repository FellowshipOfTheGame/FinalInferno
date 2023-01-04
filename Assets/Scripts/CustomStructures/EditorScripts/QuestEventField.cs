using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    public class QuestEventField {
        private const string defaultMessage = "No quest selected, condition check defaults to True";
        private SerializedProperty quest, eventFlag;
        private int questFlagIndex;
        private Rect questRect;
        public Rect QuestRect => questRect;
        private Rect eventRect;
        public Rect EventRect => eventRect;

        public float GetFieldHeight(SerializedProperty property) {
            return 2 * EditorGUIUtility.singleLineHeight;
        }

        public void FindSerializedStructProperties(SerializedProperty property) {
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
        }

        public void DrawQuestEventField(Rect position) {
            DrawQuestField(position);
            DrawEventFlagFieldOrDefaultWarning();
        }

        private void DrawQuestField(Rect position) {
            questRect = new Rect(position);
            questRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(questRect, quest);
        }

        private void DrawEventFlagFieldOrDefaultWarning() {
            eventRect = EditorUtils.NewRectBelow(questRect);
            if (quest.objectReferenceValue != null) {
                DrawEventFlagField();
            } else {
                eventFlag.stringValue = "";
                DrawDefaultValueMessage();
            }
        }

        private void DrawEventFlagField() {
            Quest _quest = quest.objectReferenceValue as Quest;
            string[] keys = _quest.GetAllFlagNames();
            int indexOfSerializedFlag = System.Array.IndexOf(keys, eventFlag.stringValue);
            questFlagIndex = Mathf.Clamp(indexOfSerializedFlag, 0, Mathf.Max(keys.Length - 1, 0));
            questFlagIndex = EditorGUI.Popup(eventRect, "Event", questFlagIndex, keys);
            eventFlag.stringValue = (keys.Length > 0) ? keys[questFlagIndex] : "";
        }

        private void DrawDefaultValueMessage() {
            EditorGUI.LabelField(eventRect, new GUIContent(defaultMessage, defaultMessage));
        }
    }
#endif
}
