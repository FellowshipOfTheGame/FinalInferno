using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
    [CreateAssetMenu(fileName = "OverworldSkillEffect", menuName = "ScriptableObject/SkillEffect/Overworld/OverworldSkillEffect")]
    public class OverworldSkillEffect : SkillEffect {
        [TextArea, SerializeField] private string description;
        public override string Description {
            get {
                string parsedDescription = description.Replace("{value1}", value1.ToString("0.##"));
                return parsedDescription.Replace("{value2}", value2.ToString("0.##"));
            }
        }

        public override void Apply(BattleUnit source, BattleUnit target) { }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OverworldSkillEffect))]
    public class OverworldSkillEffectEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            serializedObject.Update();
            OverworldSkillEffect skillEffect = target as OverworldSkillEffect;
            GUILayout.Label("Parsed description");
            GUI.enabled = false;
            GUILayout.TextArea(skillEffect.Description);
            GUI.enabled = true;
        }

    }
#endif
}
