#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno {
#if UNITY_EDITOR
    public class UpdateScriptsInScene : IProcessSceneWithReport {
        public int callbackOrder => 0;


        [MenuItem("Tools/Final Inferno/Update All Updatable Scripts in All Scenes")]
        public static void UpdateScriptsInScenes() {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            Scene currentScene = EditorSceneManager.GetActiveScene();
            for (int sceneIndex = 0; sceneIndex < EditorSceneManager.sceneCountInBuildSettings; EditorSceneManager.GetSceneByBuildIndex(sceneIndex)) {
                Scene updatedScene = EditorSceneManager.GetSceneByBuildIndex(sceneIndex);
                EditorSceneManager.LoadScene(updatedScene.name, LoadSceneMode.Single);
                UpdateAllUpdatables();
                EditorSceneManager.SaveOpenScenes();
            }
            EditorSceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
        }

        private static void UpdateAllUpdatables() {
            foreach (MonoBehaviour component in GameObject.FindObjectsOfType<MonoBehaviour>()) {
                if (!(component is IUpdatableScript))
                    continue;
                (component as IUpdatableScript).UpdateThisObject();
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report) {
            UpdateAllUpdatables();
        }
    }
#endif
}
