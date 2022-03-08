using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(QuestEvent))]
    public class QuestEventDrawer : PropertyDrawer {
        private QuestEventField questEventField = new QuestEventField();
        private const float marginSize = 5f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return questEventField.GetFieldHeight(property) + marginSize;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            questEventField.FindSerializedStructProperties(property);
            questEventField.DrawQuestEventField(position);
            EditorGUI.EndProperty();
        }
    }
#endif
}