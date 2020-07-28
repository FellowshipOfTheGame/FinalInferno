using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Final Inferno/Build Database")]
    static void BuildAllAssetBundles()
    {   
        string[] objectsFound = AssetDatabase.FindAssets("t:" + typeof(FinalInferno.AssetManager));
        int i = 0;
        foreach(string guid in objectsFound){
            FinalInferno.AssetManager database = AssetDatabase.LoadAssetAtPath<FinalInferno.AssetManager>(AssetDatabase.GUIDToAssetPath(guid));
            database.BuildDatabase();
            UnityEngine.Debug.Log("Updated database in file " + database.name);
            i++;
            if(i > 1){
                UnityEngine.Debug.LogWarning("Only one file of this type is needed");
            }
        }
    }
}