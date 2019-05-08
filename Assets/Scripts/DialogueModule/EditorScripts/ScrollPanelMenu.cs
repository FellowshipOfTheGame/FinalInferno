using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScrollPanelMenu : MonoBehaviour
{
    [MenuItem("GameObject/UI/FOG.Dialogue - ScrollPanel", false, 49)]
    static void CreateScrollPanel(MenuCommand menuCommand){
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        // Do stuff
        
        // Select newly created object
        Selection.activeObject = go;
    }
}
