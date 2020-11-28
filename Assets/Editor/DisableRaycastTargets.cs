using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public static class DisableRaycastTargets
{
    [MenuItem("Tools/Final Inferno/Disable Raycast Targets")]
    static void DisableAllRaycastTargets(){
        bool madeChanges = false;
        foreach(GraphicRaycaster raycaster in GameObject.FindObjectsOfType<GraphicRaycaster>()){
            if(raycaster.enabled) Debug.Log($"disabling raycaster {raycaster}");
            madeChanges = raycaster.enabled || madeChanges;
            raycaster.enabled = false;
        }
        foreach(Graphic graphicElement in GameObject.FindObjectsOfType<Graphic>()){
            if(graphicElement.raycastTarget) Debug.Log($"disabling raycast target {graphicElement}");
            madeChanges = graphicElement.raycastTarget || madeChanges;
            graphicElement.raycastTarget = false;
        }
        if(madeChanges){
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
