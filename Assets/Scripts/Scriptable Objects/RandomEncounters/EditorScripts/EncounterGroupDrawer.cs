using System.Collections;
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

        private Texture2D GetCroppedTexture(Sprite sprite){
            Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                      (int)sprite.textureRect.y,
                                                      (int)sprite.textureRect.width,
                                                      (int)sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
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

                Vector2 portraitSize = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
                for(int i = 0; i < 4; i++){
                    Enemy enemy = enemies[i].objectReferenceValue as Enemy;
                    if(enemy == null) continue;

                    Vector2 portraitPosition = new Vector2(displayRect.position.x + i * (portraitSize.x + 25f), displayRect.y);
                    Rect portraitRect = new Rect(portraitPosition, portraitSize);
                    Sprite enemySprite = enemy.GetSubUnitSprite(i);

                    EditorGUI.DrawTextureTransparent(portraitRect, GetCroppedTexture(enemySprite), ScaleMode.ScaleToFit);
                }
            }

            EditorGUI.EndProperty();
        }
    }
    #endif
}
