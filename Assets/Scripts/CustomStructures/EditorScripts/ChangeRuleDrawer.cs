using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar ChangeRule no editor da unity
    [CustomPropertyDrawer(typeof(ChangeRule))]
    public class ChangeRuleDrawer : PropertyDrawer {

        private SerializedProperty quest, eventFlag, animationFlag, toggleValue;
        private int index, index2;
        private bool toggle;
        private Rect questRect;
        private Rect eventRect;
        private Rect animRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            Animator _anim = Selection.activeGameObject.GetComponent<Animator>();
            int i = 1 + ((_quest != null && _quest.objectReferenceValue != null) ? 1 : 0) + ((_anim != null && _anim.runtimeAnimatorController != null) ? 1 : 0);
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            animationFlag = property.FindPropertyRelative("animationFlag");
            toggleValue = property.FindPropertyRelative("newValue");
            toggle = toggleValue.boolValue;
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null) ? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if (quest.objectReferenceValue != null) {
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = _quest.FlagNames;
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length - 1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0) ? keys[index] : "";
            } else {
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }
            // Cria o campo de referencia de animation
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            anim.enabled = false; // asjdhaisduhaloisfbhaslkdjhaskfjhbaskdjahsbnçakjsdbaksdjbasçdajsnd
            anim.enabled = true; // alskdj amsdhna sdcnhdhvasdcjasdcjmasldkc ajsldaml  unity pls
            if (anim == null || anim.runtimeAnimatorController == null) {
                animRect = new Rect(eventRect);
                animationFlag.stringValue = "";
                anim = null;
            } else {
                animRect = new Rect(new Vector2(eventRect.x, eventRect.y + eventRect.height), new Vector2(position.size.x - 40, EditorGUIUtility.singleLineHeight));
                Rect toggleRect = new Rect(new Vector2(animRect.xMax, animRect.position.y), new Vector2(position.size.x - animRect.size.x, EditorGUIUtility.singleLineHeight));
                AnimatorControllerParameter[] allParamaters = anim.parameters;
                List<string> parameters = new List<string>();
                foreach (AnimatorControllerParameter param in allParamaters) {
                    if (param.type == AnimatorControllerParameterType.Bool) {
                        parameters.Add(param.name);
                    }
                }
                index2 = Mathf.Clamp(parameters.IndexOf(animationFlag.stringValue), 0, Mathf.Max(parameters.Count - 1, 0));
                index2 = EditorGUI.Popup(animRect, "Animation flag", index2, parameters.ToArray());
                animationFlag.stringValue = (parameters.Count > 0) ? parameters[index2] : "";
                toggle = EditorGUI.Toggle(toggleRect, "", toggle);
                toggleValue.boolValue = toggle;
                anim = null;
            }

            EditorGUI.EndProperty();
        }
    }

#endif

}