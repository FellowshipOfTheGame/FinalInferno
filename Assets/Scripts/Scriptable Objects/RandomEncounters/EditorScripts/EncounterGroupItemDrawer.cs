using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EncounterGroupItem))]
    public class EncounterGroupItemDrawer : PropertyDrawer {
        private SerializedProperty group;
        private SerializedProperty chanceMultiplier;
        private const float marginSize = 5f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            bool isNull = property.FindPropertyRelative("group")?.objectReferenceValue == null;
            return ((2 * marginSize) + EditorGUIUtility.singleLineHeight + (isNull? 0f : EncounterGroupDrawer.GroupDetailsHeight));
        }

        public override void OnGUI(Rect propertyRect, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(propertyRect, label, property);
            FindSerializedFieldProperties(property);
            Rect encounterGroupRect = DrawEncounterGroupField(propertyRect, label);
            DrawMultiplierFieldOrSetDefault(propertyRect, encounterGroupRect);
            EditorGUI.EndProperty();
        }

        private void FindSerializedFieldProperties(SerializedProperty property) {
            group = property.FindPropertyRelative("group");
            chanceMultiplier = property.FindPropertyRelative("chanceMultiplier");
        }

        private Rect DrawEncounterGroupField(Rect propertyRect, GUIContent label) {
            Rect encounterGroupRect = new Rect(propertyRect);
            encounterGroupRect.width *= (group.objectReferenceValue != null) ? 0.8f : 1.0f;
            EditorGUI.PropertyField(encounterGroupRect, group, label);
            return encounterGroupRect;
        }

        private void DrawMultiplierFieldOrSetDefault(Rect propertyRect, Rect encounterGroupRect) {
            if (group.objectReferenceValue != null) {
                DrawMultiplierFieldAndLabel(propertyRect, encounterGroupRect);
            } else {
                chanceMultiplier.floatValue = 1.0f;
            }
        }

        private void DrawMultiplierFieldAndLabel(Rect propertyRect, Rect encounterGroupRect) {
            Rect chanceMultRect = CalculateMultiplierRect(propertyRect, encounterGroupRect);
            Rect chanceLabelRect = CalculateLabelRect(chanceMultRect);
            EditorGUI.LabelField(chanceLabelRect, "Multiplier");
            float value = EditorGUI.FloatField(chanceMultRect, chanceMultiplier.floatValue, EditorStyles.numberField);
            chanceMultiplier.floatValue = Mathf.Clamp(value, EncounterGroupItem.chanceMultiplierMinValue, EncounterGroupItem.chanceMultiplierMaxValue);
        }

        private Rect CalculateMultiplierRect(Rect propertyRect, Rect encounterGroupRect) {
            Rect chanceMultRect = new Rect(propertyRect);
            chanceMultRect.height = EditorGUIUtility.singleLineHeight;
            chanceMultRect.width -= encounterGroupRect.width;
            float height = (2 * marginSize) + EditorGUIUtility.singleLineHeight + EncounterGroupDrawer.GroupDetailsHeight;
            chanceMultRect.position += new Vector2(encounterGroupRect.width, height / 2.0f);
            return chanceMultRect;
        }

        private static Rect CalculateLabelRect(Rect chanceMultRect) {
            Rect chanceLabelRect= new Rect(chanceMultRect);
            chanceLabelRect.position -= new Vector2(0f, EditorGUIUtility.singleLineHeight);
            return chanceLabelRect;
        }
    }
#endif
}
