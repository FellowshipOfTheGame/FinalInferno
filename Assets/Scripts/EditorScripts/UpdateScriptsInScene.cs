#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
#endif
using Scene = UnityEngine.SceneManagement.Scene;
using System;

namespace FinalInferno {
#if UNITY_EDITOR
    public class UpdateScriptsInScene : IProcessSceneWithReport {
        private static Type[] typesToCheck = { };
        public int callbackOrder => 0;

        [MenuItem("Tools/Final Inferno/Update All Updatable Scripts in All Scenes")]
        public static void UpdateScriptsInScenes() {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            string[] guidList = AssetDatabase.FindAssets($"t:SceneAsset");
            List<string> scenesToCheck = new List<string>();
            foreach (string guid in guidList) {
                scenesToCheck.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            string currentScene = EditorSceneManager.GetActiveScene().path;
            foreach (string scenePath in scenesToCheck) {
                Scene updatedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                UpdateAllUpdatables();
                EditorSceneManager.SaveScene(updatedScene);
            }
            EditorSceneManager.OpenScene(currentScene, OpenSceneMode.Single);
        }

        private static void UpdateAllUpdatables() {
            List<UnityEngine.Object> objectsToCheck = new List<UnityEngine.Object>();
            foreach (Type type in typesToCheck) {
                objectsToCheck.AddRange(UnityEngine.Object.FindObjectsOfType(type));
            }

            foreach (UnityEngine.Object obj in objectsToCheck) {
                if (!(obj is IUpdatableScript))
                    continue;
                (obj as IUpdatableScript).UpdateThisObject();
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report) {
            UpdateAllUpdatables();
        }
    }
#endif
}
