using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomEditor(typeof(EncounterGroup))]
    public class EncounterGroupEditor : Editor {
        private SerializedProperty enemyA;
        private SerializedProperty enemyB;
        private SerializedProperty enemyC;
        private SerializedProperty enemyD;
        private SerializedProperty difficultyRating;
        private SerializedProperty canEncounter;

        private const float PORTRAIT_SIZE = 48f;

        public void OnEnable() {
            FindSerializedFieldProperties();
        }

        private void FindSerializedFieldProperties() {
            enemyA = serializedObject.FindProperty("enemyA");
            enemyB = serializedObject.FindProperty("enemyB");
            enemyC = serializedObject.FindProperty("enemyC");
            enemyD = serializedObject.FindProperty("enemyD");
            difficultyRating = serializedObject.FindProperty("difficultyRating");
            canEncounter = serializedObject.FindProperty("canEncounter");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawEnemyFieldAndShowInfo(enemyA, 0);
            DrawEnemyFieldAndShowInfo(enemyB, 1);
            DrawEnemyFieldAndShowInfo(enemyC, 2);
            DrawEnemyFieldAndShowInfo(enemyD, 3);
            EditorGUILayout.PropertyField(difficultyRating);
            EditorGUILayout.Space();
            DrawEncounterByLevelToggles();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEnemyFieldAndShowInfo(SerializedProperty enemyProperty, int enemyIndex) {
            EditorGUILayout.PropertyField(enemyProperty);
            Enemy enemy = enemyProperty.objectReferenceValue as Enemy;
            ShowEnemyInfo(enemy, enemyIndex);
            EditorUtils.DrawSeparator(EditorGUILayout.GetControlRect());
        }

        private void ShowEnemyInfo(Enemy enemy, int index) {
            if (enemy == null) {
                return;
            }
            EditorGUILayout.BeginHorizontal();
            WriteEnemyName(enemy);
            Rect currentRect = DrawEnemyPortrait(enemy, index);
            currentRect = DrawEnemyElement(enemy, currentRect);
            DrawEnemyDamageType(enemy, currentRect);
            EditorGUILayout.EndHorizontal();
        }

        private static void WriteEnemyName(Enemy enemy) {
            EditorGUILayout.LabelField($"{enemy.DialogueName}", GUILayout.MinHeight(PORTRAIT_SIZE), GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
        }

        private static Rect DrawEnemyPortrait(Enemy enemy, int index) {
            Rect currentRect = EditorGUILayout.GetControlRect();
            currentRect.size = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
            Sprite enemySprite = enemy.GetSubUnitPortrait(index);
            EditorGUI.DrawTextureTransparent(currentRect, EditorUtils.GetCroppedTexture(enemySprite), ScaleMode.ScaleToFit);
            return currentRect;
        }

        private static Rect DrawEnemyElement(Enemy enemy, Rect currentRect) {
            float padding = 5f;
            float xSize = EditorGUIUtility.currentViewWidth - (PORTRAIT_SIZE + EditorGUIUtility.labelWidth + padding);
            currentRect.size = new Vector2(xSize, PORTRAIT_SIZE / 2f);
            currentRect.position += new Vector2(PORTRAIT_SIZE + padding, 0);
            EditorGUI.LabelField(currentRect, $"Element : {enemy.Element.ToString()}");
            return currentRect;
        }

        private static void DrawEnemyDamageType(Enemy enemy, Rect currentRect) {
            currentRect.position += new Vector2(0, PORTRAIT_SIZE / 2f);
            EditorGUI.LabelField(currentRect, $"Dmg Type: {enemy.DamageFocus.ToString()}");
        }

        private void DrawEncounterByLevelToggles() {
            float windowWidth = EditorGUIUtility.currentViewWidth;
            EditorGUILayout.BeginHorizontal();
            float marginSize = 10f;
            for (int level = 1; level <= 5; level++) {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField($"Level {level}", GUILayout.ExpandWidth(true), GUILayout.MaxWidth(windowWidth / 5f - marginSize));
                DrawToggleField(level);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToggleField(int level) {
            float padding = 15f;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(padding, false);
            SerializedProperty indexElement = canEncounter.GetArrayElementAtIndex(level-1);
            indexElement.boolValue = EditorGUILayout.Toggle(indexElement.boolValue, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
