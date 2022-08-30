using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomEditor(typeof(SkillEffect), true)]
    public class SkillEffectEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            serializedObject.Update();
            SkillEffect skillEffect = target as SkillEffect;
            GUILayout.Label("Parsed description");
            GUI.enabled = false;
            GUILayout.TextArea(skillEffect.Description);
            GUI.enabled = true;
        }

    }
#endif
}
