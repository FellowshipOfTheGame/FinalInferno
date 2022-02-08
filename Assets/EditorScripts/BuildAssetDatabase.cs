#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

#if UNITY_EDITOR
public class CreateAssetDatabase : IPreprocessBuildWithReport {
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) {
        BuildDatabase();
    }

    [MenuItem("Assets/Final Inferno/Build Database")]
    private static void BuildDatabase() {
        string[] objectsFound = AssetDatabase.FindAssets("t:" + typeof(FinalInferno.AssetManager));
        int i = 0;
        foreach (string guid in objectsFound) {
            FinalInferno.AssetManager database = AssetDatabase.LoadAssetAtPath<FinalInferno.AssetManager>(AssetDatabase.GUIDToAssetPath(guid));
            database.BuildDatabase();
            Debug.Log("Updated database in file " + database.name);
            i++;
            if (i > 1) {
                Debug.LogWarning("Only one file of this type is needed");
            }
        }
        if (i == 0) {
            Debug.LogWarning("No database found to update");
        }
    }
}
#endif