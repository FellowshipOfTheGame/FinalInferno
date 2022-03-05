#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno {
    [CustomEditor(typeof(TriggerSceneChange))]
    public class TriggerSceneChangeEditor : Editor {
        private SerializedProperty sceneName;
        private Object sceneObj;
        private readonly string[] foldersToSearch = { "Assets/Scenes" };

        public void OnEnable() {
            sceneName = serializedObject.FindProperty("sceneName");
            FindSerializedSceneByName();
        }

        private void FindSerializedSceneByName() {
            string[] objectsFound = FindScenesInFolders();
            bool foundAtLeastOneScene = objectsFound != null && objectsFound.Length > 0 && !string.IsNullOrEmpty(objectsFound[0]);
            if (!string.IsNullOrEmpty(sceneName.stringValue) && foundAtLeastOneScene) {
                sceneObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectsFound[0]), typeof(Object));
            } else {
                sceneObj = null;
            }
        }

        private string[] FindScenesInFolders() {
            return AssetDatabase.FindAssets($"{sceneName.stringValue} t:sceneAsset", foldersToSearch);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawSceneSelectionField();
            DrawSceneChangeAndCutsceneFieldsIfNecessary();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSceneChangeAndCutsceneFieldsIfNecessary() {
            if (sceneObj != null) {
                bool isCutscene = DrawSceneChangeFields();
                DrawCutsceneFieldsIfNecessary(isCutscene);
            }
        }

        private void DrawSceneSelectionField() {
            sceneObj = EditorGUILayout.ObjectField(sceneObj, typeof(SceneAsset), false);
            if (sceneObj != null) {
                sceneName.stringValue = sceneObj.name;
            } else {
                sceneName.stringValue = string.Empty;
            }
        }

        private bool DrawSceneChangeFields() {
            SerializedProperty positionOnLoad = serializedObject.FindProperty("positionOnLoad");
            SerializedProperty isCutscene = serializedObject.FindProperty("isCutscene");
            SerializedProperty decision = serializedObject.FindProperty("decision");
            EditorGUILayout.PropertyField(positionOnLoad);
            EditorGUILayout.PropertyField(isCutscene);
            EditorGUILayout.PropertyField(decision);
            return isCutscene.boolValue;
        }

        private void DrawCutsceneFieldsIfNecessary(bool isCutscene) {
            if (!isCutscene) {
                return;
            }
            SerializedProperty saveGamePosition = serializedObject.FindProperty("saveGamePosition");
            SerializedProperty dialogues = serializedObject.FindProperty("dialogues");
            EditorGUILayout.PropertyField(saveGamePosition);
            EditorGUILayout.PropertyField(dialogues, includeChildren: true);
        }
    }
}
#endif
