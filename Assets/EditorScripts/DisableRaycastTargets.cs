using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.UI;

#if UNITY_EDITOR
public static class DisableRaycastTargets {
    [MenuItem("Tools/Final Inferno/Disable Raycast Targets")]
    private static void DisableAllRaycastTargets() {
        bool madeChanges = false;
        foreach (GraphicRaycaster raycaster in GameObject.FindObjectsOfType<GraphicRaycaster>()) {
            if (raycaster.enabled) {
                Debug.Log($"disabling raycaster {raycaster}");
            }

            madeChanges = raycaster.enabled || madeChanges;
            raycaster.enabled = false;
        }
        foreach (Graphic graphicElement in GameObject.FindObjectsOfType<Graphic>()) {
            if (graphicElement.raycastTarget) {
                Debug.Log($"disabling raycast target {graphicElement}");
            }

            madeChanges = graphicElement.raycastTarget || madeChanges;
            graphicElement.raycastTarget = false;
        }
        if (madeChanges) {
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
#endif
