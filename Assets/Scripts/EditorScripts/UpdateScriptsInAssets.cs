#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    public class UpdateScriptsInAssets : IPreprocessBuildWithReport {
        public int callbackOrder => 0;

        [MenuItem("Tools/Final Inferno/Update All Updatable Scripts in All Assets")]
        public static void UpdateAllAssets() {
            UpdateAllScriptableObjects(true);
            UpdateAllPrefabs();
        }

        private static void UpdateAllPrefabs() {
            Debug.LogWarning("Prefab update is not implemented yet");
        }

        private static void UpdateAllScriptableObjects(bool shouldSetDirty) {
            string[] objectsFound = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject)}");
            foreach (string guid in objectsFound) {
                ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (!(scriptableObject is IUpdatableScript))
                    continue;
                (scriptableObject as IUpdatableScript).UpdateThisObject();
                if (shouldSetDirty)
                    EditorUtility.SetDirty(scriptableObject);
            }
            if (shouldSetDirty)
                AssetDatabase.SaveAssets();
        }

        public void OnPreprocessBuild(BuildReport report) {
            UpdateAllScriptableObjects(false);
        }
    }
#endif
}
