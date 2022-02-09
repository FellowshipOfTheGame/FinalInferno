using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomPropertyDrawer(typeof(QuestEvent))]
    public class QuestEventDrawer : PropertyDrawer {

        private SerializedProperty quest, eventFlag;
        private int index;
        private Rect questRect;
        private Rect eventRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null) ? 1 : (_quest.objectReferenceValue == null) ? 1 : 2;
            return (i * EditorGUIUtility.singleLineHeight) + 5f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
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

            EditorGUI.EndProperty();
        }
    }

#endif

}