using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que muda o estado de um botão.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Scene Transition")]
    public class SceneTransition : Action
    {
        [SerializeField] private string sceneName = "DebuggingDialogue";
        /// <summary>
        /// Executa uma ação.
        /// Muda o estado do botão.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>
        public override void Act(StateController controller)
        {
            // if(sceneName == "")
            //     SceneLoader.LoadOWScene(SceneLoader.LastOWScene);
            // else
            //     SceneLoader.LoadOWScene(sceneName);
            SceneLoader.LoadOWScene(SceneLoader.LastOWSceneID, true);
        }

    }

    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomEditor(typeof(SceneTransition))]
    public class SceneTransitionEditor : Editor{
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
