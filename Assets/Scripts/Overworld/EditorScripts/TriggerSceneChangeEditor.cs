#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
    [CustomEditor(typeof(TriggerSceneChange))]
    public class TriggerSceneChangeEditor : Editor {
        private SerializedProperty sceneName;
        private Object sceneObj;
        private readonly string[] foldersToSearch = { "Assets/Scenes" };

        public void OnEnable() {
            sceneObj = null;
            sceneName = serializedObject.FindProperty("sceneName");
            bool hasSerializedSceneName = !string.IsNullOrEmpty(sceneName.stringValue);
            if (hasSerializedSceneName) {
                FindSerializedSceneByName();
            }
        }

        private void FindSerializedSceneByName() {
            string[] objectsFound = FindScenesInFolders();
            if (FoundAtLeastOneScene(objectsFound)) {
                sceneObj = LoadAssetWithGUID(objectsFound[0]);
            }
        }

        private string[] FindScenesInFolders() {
            return AssetDatabase.FindAssets($"{sceneName.stringValue} t:sceneAsset", foldersToSearch);
        }

        private static bool FoundAtLeastOneScene(string[] objectsFound) {
            return objectsFound != null && objectsFound.Length > 0 && !string.IsNullOrEmpty(objectsFound[0]);
        }

        private static Object LoadAssetWithGUID(string objectGUID) {
            return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectGUID), typeof(Object));
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
            sceneName.stringValue = sceneObj?.name ?? string.Empty;
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
