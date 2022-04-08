using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno.EventSystem {
#if UNITY_EDITOR
    [CustomEditor(typeof(EventFI), editorForChildClasses: true)]
    public class EventFIEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Force Raise")) {
                (target as EventFI).Raise();
            }
        }
    }
#endif
}
