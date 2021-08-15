using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
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

        public void OnEnable(){
            enemyA = serializedObject.FindProperty("enemyA");
            enemyB = serializedObject.FindProperty("enemyB");
            enemyC = serializedObject.FindProperty("enemyC");
            enemyD = serializedObject.FindProperty("enemyD");
            difficultyRating = serializedObject.FindProperty("difficultyRating");
            canEncounter = serializedObject.FindProperty("canEncounter");
        }

        private void ShowEnemyInfo(Enemy enemy, int index){
            if(enemy == null) return;

            EditorGUILayout.BeginHorizontal();
            // Name
            EditorGUILayout.LabelField($"{enemy.DialogueName}", GUILayout.MinHeight(PORTRAIT_SIZE));
            // Portrait
            Rect currentRect = EditorGUILayout.GetControlRect();
            currentRect.size = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
            Sprite enemySprite = enemy.GetSubUnitPortrait(index);
            EditorGUI.DrawTextureTransparent(currentRect, EditorUtils.GetCroppedTexture(enemySprite), ScaleMode.ScaleToFit);
            // Element
            // DamageType
            EditorGUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();

            Enemy enemy;
            EditorGUILayout.PropertyField(enemyA);
            enemy = enemyA.objectReferenceValue as Enemy;
            ShowEnemyInfo(enemy, 0);
            EditorGUILayout.PropertyField(enemyB);
            enemy = enemyB.objectReferenceValue as Enemy;
            ShowEnemyInfo(enemy, 1);
            EditorGUILayout.PropertyField(enemyC);
            enemy = enemyC.objectReferenceValue as Enemy;
            ShowEnemyInfo(enemy, 2);
            EditorGUILayout.PropertyField(enemyD);
            enemy = enemyD.objectReferenceValue as Enemy;
            ShowEnemyInfo(enemy, 3);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(difficultyRating);
            EditorGUILayout.Space();

            float windowWidth = EditorGUIUtility.currentViewWidth;
            EditorGUILayout.BeginHorizontal();
            for(int i = 0; i < 5; i++){
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField($"Level {i}", GUILayout.ExpandWidth(true), GUILayout.MaxWidth(windowWidth/5f - 10f));

                    EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space(15f, false);
                        SerializedProperty indexElement = canEncounter.GetArrayElementAtIndex(i);
                        indexElement.boolValue = EditorGUILayout.Toggle(indexElement.boolValue, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
