using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace FinalInferno{
    public class TriggerSceneChange : Triggerable
    {
        [SerializeField] private string sceneName = "Battle";
        [SerializeField] private Vector2 positionOnLoad = new Vector2(0,0);
        [SerializeField] private bool isCutscene = false;
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        
        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            if(sceneName != null && sceneName != ""){
                Fog.Dialogue.Dialogue selectedDialogue = null;
                if(isCutscene){
                    foreach(DialogueEntry entry in dialogues){
                        //Debug.Log("Checking dialogue: " + entry.dialogue + " with quest " + entry.quest + " and event " + entry.eventFlag);
                        if(entry.quest.events[entry.eventFlag]){
                            selectedDialogue = entry.dialogue;
                        }else{
                            //Debug.Log("Event " + entry.eventFlag + " deu false");
                            break;
                        }
                    }
                }
                //Debug.Log("Loading scene: " + sceneName + "; dialogue = " + selectedDialogue);
                if(!isCutscene)
                    SceneLoader.LoadOWScene(sceneName, true, positionOnLoad);
                else
                    SceneLoader.LoadCustscene(sceneName, selectedDialogue);
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

            if(sceneObj != null){
                SerializedProperty positionOnLoad = serializedObject.FindProperty("positionOnLoad");
                SerializedProperty isCutscene = serializedObject.FindProperty("isCutscene");
                EditorGUILayout.PropertyField(positionOnLoad);
                EditorGUILayout.PropertyField(isCutscene);
                if(isCutscene.boolValue){
                    SerializedProperty dialogues = serializedObject.FindProperty("dialogues");
                    EditorGUILayout.PropertyField(dialogues, includeChildren:true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
