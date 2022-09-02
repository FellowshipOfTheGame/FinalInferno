using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno.EventSystem {
#if UNITY_EDITOR
    [CustomEditor(typeof(IntEventFI), editorForChildClasses: true)]
    public class IntEventFIEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Force Raise")) {
                IntEventFI eventSO = target as IntEventFI;
                SerializedProperty debugValue = serializedObject.FindProperty("debugValue");
                eventSO.Raise(debugValue.intValue);
            }
        }
    }
#endif
}