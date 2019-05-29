using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace FinalInferno{
    public class TriggerSceneChange : Triggerable
    {
        [SerializeField] private string sceneName = "Battle";
        
        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            if(sceneName != null && sceneName != ""){
                SceneLoader.LoadOWScene(sceneName);
            }
        }
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomEditor(typeof(TriggerSceneChange))]
    public class TriggerSceneChangeEditor : Editor{
        SerializedProperty sceneName;
        Object sceneObj;

        public void OnEnable(){
            sceneName = serializedObject.FindProperty("sceneName");
            //Debug.Log("Procurando " + sceneName.stringValue +  "...");
            string[] objectsFound = AssetDatabase.FindAssets(sceneName.stringValue, new[] {"Assets/Scenes"});
            if(sceneName.stringValue != "" && objectsFound != null && objectsFound.Length > 0 && objectsFound[0] != ""){
                sceneObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectsFound[0]), typeof(Object));
            }else{
                //Debug.Log("Não achou");
                sceneObj = null;
            }
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();

            sceneObj = EditorGUILayout.ObjectField(sceneObj, typeof(SceneAsset), false);
            if(sceneObj != null)
                sceneName.stringValue = sceneObj.name;
            else
                sceneName.stringValue = "";

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
