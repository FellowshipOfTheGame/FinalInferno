using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno{
    public class OverworldObjectsMenu : MonoBehaviour
    {
        [MenuItem("GameObject/FinalInferno/Overworld Objects", false, 48)]
        static void CreateOverworldObjects(MenuCommand menuCommand){
            List<GameObject> prefabs = new List<GameObject>();
            //GameObject[] prefabs = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/Overworld/") as GameObject[];
            GameObject[] objectList = new GameObject[6];
            foreach(GameObject prefab in prefabs){
                switch(prefab.name){
                    case "Main Camera":
                        objectList[0] = prefab;
                        break;
                    case "Random Encounter Calculator":
                        objectList[1] = prefab;
                        break;
                    case "Character 1":
                        objectList[2] = prefab;
                        break;
                    case "Character 2":
                        objectList[3] = prefab;
                        break;
                    case "Character 3":
                        objectList[4] = prefab;
                        break;
                    case "Character 4":
                        objectList[5] = prefab;
                        break;
                }
            }
            foreach(GameObject go in objectList){
                GameObject newObj = Instantiate(go);
                GameObjectUtility.SetParentAndAlign(newObj, menuCommand.context as GameObject);
                Undo.RegisterCreatedObjectUndo(newObj, "Create " + newObj.name);
                if(go.name == "Random Encounter Calculator")
                    Selection.activeObject = newObj;
            }
        }
    }
}
