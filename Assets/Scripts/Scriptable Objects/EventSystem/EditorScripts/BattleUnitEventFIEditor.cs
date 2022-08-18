using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno.EventSystem {
#if UNITY_EDITOR
    [CustomEditor(typeof(BattleUnitEventFI), editorForChildClasses: true)]
    public class BattleUnitEventFIEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Force Raise")) {
                BattleUnitEventFI eventSO = target as BattleUnitEventFI;
                SerializedProperty debugValue = serializedObject.FindProperty("debugValue");
                eventSO.Raise(debugValue.objectReferenceValue as BattleUnit);
            }
        }
    }
#endif
}
