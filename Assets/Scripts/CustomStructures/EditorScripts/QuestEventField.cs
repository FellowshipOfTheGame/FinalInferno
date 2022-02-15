using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
    #if UNITY_EDITOR
    public class QuestEventField {
        private SerializedProperty quest, eventFlag;
        private int questFlagIndex;
        private Rect questRect;
        public Rect QuestRect => questRect;
        private Rect eventRect;
        public Rect EventRect => eventRect;

        public float GetFieldHeight(SerializedProperty property) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int nLines = 1 + ((_quest != null && _quest.objectReferenceValue != null) ? 1 : 0);
            return (nLines * EditorGUIUtility.singleLineHeight);
        }

        public void DrawQuestEventField(Rect position) {
            DrawQuestField(position);
            DrawEventFlagFieldIfNecessary();
        }

        public void FindSerializedStructProperties(SerializedProperty property) {
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
        }

        private void DrawQuestField(Rect position) {
            questRect = new Rect(position);
            questRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(questRect, quest);
        }

        private void DrawEventFlagFieldIfNecessary() {
            eventRect = (quest.objectReferenceValue == null) ? questRect : EditorUtils.NewRectBelow(questRect);
            if (quest.objectReferenceValue != null) {
                DrawEventFlagField();
            } else {
                eventFlag.stringValue = "";
            }
        }

        private void DrawEventFlagField() {
            Quest _quest = quest.objectReferenceValue as Quest;
            string[] keys = _quest.FlagNames;
            int indexOfSerializedFlag = System.Array.IndexOf(keys, eventFlag.stringValue);
            questFlagIndex = Mathf.Clamp(indexOfSerializedFlag, 0, Mathf.Max(keys.Length - 1, 0));
            questFlagIndex = EditorGUI.Popup(eventRect, "Event", questFlagIndex, keys);
            eventFlag.stringValue = (keys.Length > 0) ? keys[questFlagIndex] : "";
        }
    }
#endif
}
