using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace FinalInferno {
    [CustomEditor(typeof(AssetManager))]
    public class AssetManagerEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawOnlyReadInteractions();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawOnlyReadInteractions() {
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
}
#endif