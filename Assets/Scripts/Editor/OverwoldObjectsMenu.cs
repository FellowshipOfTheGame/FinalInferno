using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno{
    public class OverworldObjectsMenu : MonoBehaviour
    {
        [MenuItem("GameObject/FinalInferno/Overworld Objects", false, 48)]
        static void CreateOverworldObjects(MenuCommand menuCommand){
            GameObject[] objectList = new GameObject[6];
            objectList[5] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Main Camera.prefab", typeof(GameObject));
            if(!objectList[5])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Main Camera.prefab");
            objectList[4] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Random Encounter Calculator.prefab", typeof(GameObject));
            if(!objectList[4])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Random Encounter Calculator.prefab");
            objectList[3] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 1.prefab", typeof(GameObject));
            if(!objectList[3])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Character 1.prefab");
            objectList[2] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 2.prefab", typeof(GameObject));
            if(!objectList[2])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Character 2.prefab");
            objectList[1] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 3.prefab", typeof(GameObject));
            if(!objectList[1])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Character 3.prefab");
            objectList[0] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Overworld/Character 4.prefab", typeof(GameObject));
            if(!objectList[0])
                Debug.Log("Could not load Assets/Prefabs/Overworld/Character 4.prefab");
            foreach(GameObject go in objectList){
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(go);
                GameObjectUtility.SetParentAndAlign(newObj, menuCommand.context as GameObject);
                Undo.RegisterCreatedObjectUndo(newObj, "Create " + newObj.name);
                if(go.name == "Random Encounter Calculator")
                    Selection.activeObject = newObj;
            }
        }
    }
}
