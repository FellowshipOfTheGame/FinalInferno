using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EncounterGroup))]
    public class EncounterGroupDrawer : PropertyDrawer {
        private SerializedProperty difficultyRating;
        private SerializedProperty[] enemies = {null, null, null, null};
        private Rect objPickerRect, displayRect;
        private const float PORTRAIT_SIZE = 48f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            bool isNull = property.objectReferenceValue == null;
            return (10f + EditorGUIUtility.singleLineHeight * (isNull? 1f : 3f) + (isNull? 0f : PORTRAIT_SIZE+5f));
        }

        public override void OnGUI(Rect propertyRect, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(propertyRect, label, property);
            EditorGUI.PrefixLabel(propertyRect, GUIUtility.GetControlID(FocusType.Passive), label);

            objPickerRect = new Rect(new Vector2(propertyRect.position.x, propertyRect.position.y + 5f), new Vector2(propertyRect.size.x, EditorGUIUtility.singleLineHeight));
            displayRect = new Rect(new Vector2(objPickerRect.position.x, objPickerRect.position.y + objPickerRect.size.y + 2.5f), new Vector2(propertyRect.size.x, 2 * EditorGUIUtility.singleLineHeight + PORTRAIT_SIZE + 2.5f));

            EditorGUI.ObjectField(objPickerRect, property);
            if(property.objectReferenceValue != null){
                SerializedObject obj = new SerializedObject(property.objectReferenceValue);
                enemies[0] = obj.FindProperty("enemyA");
                enemies[1] = obj.FindProperty("enemyB");
                enemies[2] = obj.FindProperty("enemyC");
                enemies[3] = obj.FindProperty("enemyD");
                difficultyRating = obj.FindProperty("difficultyRating");
                ReadOnlyCollection<bool> canEncounter = (obj.targetObject as EncounterGroup).CanEncounter;

                Vector2 portraitSize = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
                for(int i = 0; i < 4; i++){
                    Enemy enemy = enemies[i].objectReferenceValue as Enemy;
                    if(enemy == null) continue;

                    Vector2 portraitPosition = new Vector2(displayRect.position.x + i * (portraitSize.x + 25f), displayRect.y);
                    Rect portraitRect = new Rect(portraitPosition, portraitSize);
                    Sprite enemySprite = enemy.GetSubUnitPortrait(i);

                    EditorGUI.DrawTextureTransparent(portraitRect, EditorUtils.GetCroppedTexture(enemySprite), ScaleMode.ScaleToFit);
                }

                Rect detailsRect = new Rect(new Vector2(displayRect.position.x, displayRect.position.y + PORTRAIT_SIZE + 2.5f), new Vector2(displayRect.size.x, 2 * EditorGUIUtility.singleLineHeight));
                Rect difficultyRect = new Rect(detailsRect.position.x, detailsRect.position.y, detailsRect.width/2f, detailsRect.height);
                Rect levelInfoRect = new Rect(detailsRect.position.x + detailsRect.width/2f, detailsRect.position.y, detailsRect.width/2f, detailsRect.height);
                EditorGUI.LabelField(difficultyRect, $"Dificulty rating: {(difficultyRating.floatValue):0.##}");
                EditorGUI.LabelField(levelInfoRect, $"Levels: 1{(canEncounter[0]?'☑':'☐')} 2{(canEncounter[1]?'☑':'☐')} 3{(canEncounter[2]?'☑':'☐')} 4{(canEncounter[3]?'☑':'☐')} 5{(canEncounter[4]?'☑':'☐')}");
            }

            EditorGUI.EndProperty();
        }
    }
    #endif
}
