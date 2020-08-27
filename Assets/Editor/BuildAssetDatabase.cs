using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAssetDatabase : IPreprocessBuildWithReport, IProcessSceneWithReport
{
    public int callbackOrder{ get => 0; }

    public void OnPreprocessBuild(BuildReport report){
        BuildDatabase();
    }

    [MenuItem("Assets/Final Inferno/Build Database")]
    static void BuildDatabase()
    {   
        string[] objectsFound = AssetDatabase.FindAssets("t:" + typeof(FinalInferno.AssetManager));
        int i = 0;
        foreach(string guid in objectsFound){
            FinalInferno.AssetManager database = AssetDatabase.LoadAssetAtPath<FinalInferno.AssetManager>(AssetDatabase.GUIDToAssetPath(guid));
            database.BuildDatabase();
            Debug.Log("Updated database in file " + database.name);
            i++;
            if(i > 1){
                Debug.LogWarning("Only one file of this type is needed");
            }
        }
        if(i == 0){
            Debug.LogWarning("No database found to update");
        }
    }

    public void OnProcessScene(Scene scene, BuildReport report){
        FinalInferno.RECalculator[] calcs = GameObject.FindObjectsOfType<FinalInferno.RECalculator>();

        foreach(FinalInferno.RECalculator calculator in calcs){
            Debug.Log($"Reloading Random Encounter table for object {calculator.gameObject.name} in scene {scene.name}");
            calculator.ReloadTable();
        }
    }
}