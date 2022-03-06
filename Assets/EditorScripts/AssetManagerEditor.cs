using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace FinalInferno {
    [CustomEditor(typeof(AssetManager))]
    public class AssetManagerEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            SetOnlyReadInteractions();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void SetOnlyReadInteractions() {
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
}
#endif