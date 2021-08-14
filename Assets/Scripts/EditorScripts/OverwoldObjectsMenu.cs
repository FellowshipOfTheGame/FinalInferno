using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno{
#if UNITY_EDITOR
    public class OverworldObjectsMenu : MonoBehaviour
    {
        [MenuItem("GameObject/FinalInferno/Overworld Objects", false, 48)]
        static void CreateOverworldObjects(MenuCommand menuCommand){
            GameObject[] objectList = new GameObject[7];
            objectList[6] = GameObject.FindObjectOfType<CameraController>()?.gameObject;
            if(!objectList[6]){
                objectList[6] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Main Camera.prefab", typeof(GameObject));
                if(!objectList[6])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Main Camera.prefab");
            }else
                objectList[6] = null;

            objectList[5] = GameObject.FindObjectOfType<Canvas>()?.gameObject;
            if(!objectList[5]){
                objectList[5] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Canvas.prefab", typeof(GameObject));
                if(!objectList[5]){
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Canvas.prefab");
                }else{
                    Canvas canvas = objectList[5].GetComponent<Canvas>();
                    if(canvas) canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
                }
            }else
                objectList[5] = null;

            objectList[4] = GameObject.FindObjectOfType<RECalculator>()?.gameObject;
            if(!objectList[4]){
                objectList[4] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Random Encounter Calculator.prefab", typeof(GameObject));
                if(!objectList[4])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Random Encounter Calculator.prefab");
            }else{
                Selection.activeObject = objectList[4];
                objectList[4] = null;
            }

            objectList[3] = GameObject.Find("Character 1");
            if(!objectList[3]){
                objectList[3] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 1.prefab", typeof(GameObject));
                if(!objectList[3])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Character 1.prefab");
            }else
                objectList[3] = null;

            objectList[2] = GameObject.Find("Character 2");
            if(!objectList[2]){
                objectList[2] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 2.prefab", typeof(GameObject));
                if(!objectList[2])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Character 2.prefab");
            }else
                objectList[2] = null;

            objectList[1] = GameObject.Find("Character 3");
            if(!objectList[1]){
                objectList[1] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 3.prefab", typeof(GameObject));
                if(!objectList[1])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Character 3.prefab");
            }else
                objectList[1] = null;
                
            objectList[0] = GameObject.Find("Character 4");
            if(!objectList[0]){
                objectList[0] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 4.prefab", typeof(GameObject));
                if(!objectList[0])
                    Debug.Log("Could not load Assets/Prefabs/Overworld/Character 4.prefab");
            }else
                objectList[0] = null;

            foreach(GameObject go in objectList){
                if(go != null){
                    GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(go);
                    GameObjectUtility.SetParentAndAlign(newObj, menuCommand.context as GameObject);
                    Undo.RegisterCreatedObjectUndo(newObj, "Create " + newObj.name);
                    if(go.name == "Random Encounter Calculator")
                        Selection.activeObject = newObj;
                }
            }
        }
    }

#endif
}
