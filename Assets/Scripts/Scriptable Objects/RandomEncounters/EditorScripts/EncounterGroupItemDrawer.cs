using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EncounterGroupItem))]
    public class EncounterGroupItemDrawer : PropertyDrawer {
        private SerializedProperty group;
        private SerializedProperty chanceMultiplier;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            bool isNull = property.FindPropertyRelative("group")?.objectReferenceValue == null;
            return (10f + EditorGUIUtility.singleLineHeight * (isNull? 1f : 3f) + (isNull? 0f : EncounterGroupDrawer.PORTRAIT_SIZE+5f));
        }

        public override void OnGUI(Rect propertyRect, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(propertyRect, label, property);

            group = property.FindPropertyRelative("group");
            chanceMultiplier = property.FindPropertyRelative("chanceMultiplier");

            Rect encounterGroupRect = new Rect(propertyRect);
            Rect chanceMultRect = new Rect(propertyRect);
            Rect chanceLabelRect = new Rect();
            if(group.objectReferenceValue != null){
                encounterGroupRect.width *= 0.8f;
                chanceMultRect.width -= encounterGroupRect.width;
                float height = 10f + EditorGUIUtility.singleLineHeight * 3f + EncounterGroupDrawer.PORTRAIT_SIZE+5f;
                chanceMultRect.position += new Vector2(encounterGroupRect.width, height/2.0f);
                chanceMultRect.height = EditorGUIUtility.singleLineHeight;
                chanceLabelRect = new Rect(chanceMultRect);
                chanceLabelRect.position -= new Vector2(0f, EditorGUIUtility.singleLineHeight);
            }
            chanceMultRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(encounterGroupRect, group, label);

            if(group.objectReferenceValue != null){
                EditorGUI.LabelField(chanceLabelRect, "Multiplier");
                float value = EditorGUI.FloatField(chanceMultRect, chanceMultiplier.floatValue, EditorStyles.numberField);
                chanceMultiplier.floatValue = Mathf.Clamp(value, 0.1f, 5f);
            }else{
                chanceMultiplier.floatValue = 1.0f;
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}
