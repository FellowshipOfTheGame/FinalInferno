using System.Collections.ObjectModel;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EncounterGroup))]
    public class EncounterGroupDrawer : PropertyDrawer {
        private SerializedProperty difficultyRating;
        private SerializedProperty[] enemies = { null, null, null, null };
        private Rect objPickerRect, displayRect;
        public const float PORTRAIT_SIZE = 48f;
        private const float marginSize = 5f;
        private const float portraitSpacing = 25f;
        public static float GroupDetailsHeight => 2f * EditorGUIUtility.singleLineHeight + PORTRAIT_SIZE + marginSize;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            bool isNull = property.objectReferenceValue == null;
            return ((2 * marginSize) + EditorGUIUtility.singleLineHeight + (isNull ? 0f : GroupDetailsHeight));
        }

        public override void OnGUI(Rect propertyRect, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(propertyRect, label, property);
            CalculateFieldRects(propertyRect);
            EditorGUI.ObjectField(objPickerRect, property, label);
            if (property.objectReferenceValue != null)
                DrawEncounterGroupPreview(propertyRect, property);
            EditorGUI.EndProperty();
        }

        private void CalculateFieldRects(Rect propertyRect) {
            Vector2 objPickerPosition = new Vector2(propertyRect.position.x, propertyRect.position.y + marginSize);
            Vector2 objPickerSize = new Vector2(propertyRect.size.x, EditorGUIUtility.singleLineHeight);
            objPickerRect = new Rect(objPickerPosition, objPickerSize);
            Vector2 displayPosition = objPickerPosition + new Vector2(0f, objPickerSize.y + marginSize / 2f);
            Vector2 displaySize = new Vector2(propertyRect.size.x, GroupDetailsHeight - marginSize / 2f);
            displayRect = new Rect(displayPosition, displaySize);
        }

        private void DrawEncounterGroupPreview(Rect propertyRect, SerializedProperty property) {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            FindSerializedFieldProperties(serializedObject);
            for (int enemyIndex = 0; enemyIndex < 4; enemyIndex++)
                DrawEnemyPortrait(propertyRect, enemyIndex);
            WriteDificultyAndLevelInfo(serializedObject);
        }

        private void FindSerializedFieldProperties(SerializedObject serializedObject) {
            enemies[0] = serializedObject.FindProperty("enemyA");
            enemies[1] = serializedObject.FindProperty("enemyB");
            enemies[2] = serializedObject.FindProperty("enemyC");
            enemies[3] = serializedObject.FindProperty("enemyD");
            difficultyRating = serializedObject.FindProperty("difficultyRating");
        }

        private void DrawEnemyPortrait(Rect propertyRect, int enemyIndex) {
            Enemy enemy = enemies[enemyIndex].objectReferenceValue as Enemy;
            if (enemy == null)
                return;

            Vector2 portraitSize = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
            float xOffset = enemyIndex * (PORTRAIT_SIZE + portraitSpacing);
            Vector2 portraitPosition = new Vector2(displayRect.position.x + xOffset, displayRect.y);
            Rect portraitRect = EditorUtils.CropRect(new Rect(portraitPosition, portraitSize), propertyRect);
            Sprite enemySprite = enemy.GetSubUnitPortrait(enemyIndex);
            EditorGUI.DrawTextureTransparent(portraitRect, EditorUtils.GetCroppedTexture(enemySprite), ScaleMode.ScaleAndCrop);
        }

        private void WriteDificultyAndLevelInfo(SerializedObject serializedObject) {
            ReadOnlyCollection<bool> canEncounter = (serializedObject.targetObject as EncounterGroup).CanEncounter;
            Vector2 detailsRectSize = new Vector2(displayRect.size.x, 2 * EditorGUIUtility.singleLineHeight);
            Vector2 detailsRectPosition = displayRect.position + new Vector2(0f, PORTRAIT_SIZE + 2.5f);
            WriteDifficulty(detailsRectPosition, detailsRectSize);
            WriteLevelInfo(canEncounter, detailsRectPosition, detailsRectSize);
        }

        private void WriteDifficulty(Vector2 detailsRectPosition, Vector2 detailsRectSize) {
            Rect difficultyRect = new Rect(detailsRectPosition, new Vector2(detailsRectSize.x / 2f, detailsRectSize.y));
            EditorGUI.LabelField(difficultyRect, $"Dificulty rating: {(difficultyRating.floatValue):0.##}");
        }

        private static void WriteLevelInfo(ReadOnlyCollection<bool> canEncounter, Vector2 detailsRectPosition, Vector2 detailsRectSize) {
            Rect levelInfoRect = new Rect(detailsRectPosition + new Vector2(detailsRectSize.x / 2f, 0f), detailsRectSize);
            EditorGUI.LabelField(levelInfoRect, $"Levels: 1{(canEncounter[0] ? '☑' : '☐')} 2{(canEncounter[1] ? '☑' : '☐')} 3{(canEncounter[2] ? '☑' : '☐')} 4{(canEncounter[3] ? '☑' : '☐')} 5{(canEncounter[4] ? '☑' : '☐')}");
        }
    }
#endif
}
