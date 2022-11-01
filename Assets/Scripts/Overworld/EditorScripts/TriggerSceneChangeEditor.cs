#if UNITY_EDITOR
using UnityEditor;

namespace FinalInferno {
    [CustomEditor(typeof(TriggerSceneChange))]
    public class TriggerSceneChangeEditor : Editor {
        SerializedProperty scene;
        SerializedProperty positionOnLoad;
        SerializedProperty isCutscene;
        SerializedProperty saveGamePosition;
        SerializedProperty dialogues;
        SerializedProperty sceneChangeInfoReference;
        SerializedProperty startSceneChangeAnimation;

        public void OnEnable() {
            scene = serializedObject.FindProperty("scene");
            positionOnLoad = serializedObject.FindProperty("positionOnLoad");
            isCutscene = serializedObject.FindProperty("isCutscene");
            saveGamePosition = serializedObject.FindProperty("saveGamePosition");
            dialogues = serializedObject.FindProperty("dialogues");
            sceneChangeInfoReference = serializedObject.FindProperty("sceneChangeInfoReference");
            startSceneChangeAnimation = serializedObject.FindProperty("startSceneChangeAnimation");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(scene);
            serializedObject.Update();
            DrawSceneChangeAndCutsceneFieldsIfNecessary();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSceneChangeAndCutsceneFieldsIfNecessary() {
            string sceneName = scene.FindPropertyRelative("sceneName")?.stringValue;
            if (string.IsNullOrEmpty(sceneName))
                return;
            DrawSceneChangeFields();
            DrawCutsceneFieldsIfNecessary();
            DrawObjectReferences();
        }

        private void DrawSceneChangeFields() {
            EditorGUILayout.PropertyField(positionOnLoad);
            EditorGUILayout.PropertyField(isCutscene);
        }

        private void DrawCutsceneFieldsIfNecessary() {
            if (!isCutscene.boolValue)
                return;
            EditorGUILayout.PropertyField(saveGamePosition);
            EditorGUILayout.PropertyField(dialogues, includeChildren: true);
        }

        private void DrawObjectReferences() {
            EditorGUILayout.PropertyField(sceneChangeInfoReference);
            EditorGUILayout.PropertyField(startSceneChangeAnimation);
        }
    }
}
#endif
