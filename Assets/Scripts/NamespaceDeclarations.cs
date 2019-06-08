using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno
{
    public enum DamageType {
        Physical,
        Magical,
        None
    }
    public enum Element {
        Fire,
        Ice,
        Wind,
        Earth,
        Neutral
    }
    public enum TargetType {
        SingleAlly,
        MultiAlly,
        SingleEnemy,
        MultiEnemy,
        Self,
        Null
    }

    public enum VictoryType {
        Nobody,
        Heroes,
        Enemys
    }

    public enum UnitType {
        Hero,
        Enemy
    }

    public enum SkillType {
        Active,
        PassiveOnSpawn,
        PassiveOnStart,
        PassiveOnEnd,
        PassiveOnGiveBuff,
        PassiveOnReceiveBuff,
        PassiveOnGiveDebuff,
        PassiveOnReceiveDebuff,
        PassiveOnGiveDamage,
        PassiveOnTakeDamage,
        PassiveOnHit,
        PassiveOnGetHit,
        PassiveOnDeath
    }

    public enum StatusType {
        Buff,
        Debuff,
        None
    }

     public struct SkillInfo{
        long xp;
        bool active;
    }

    public struct QuestInfo{
        string name;
        int flagsTrue;
    }

    public struct SaveInfo{
        long xpParty;
        List<QuestInfo> quest;
        //quests de kill
        List<string> archetype;
        List<int> hpCur;
        List<Vector2> position;
        List<List<SkillInfo> > skills;
    }

    [System.Serializable]
    public class QuestDictionary : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<string, bool>{ }

    [System.Serializable]
    public struct SkillEffectTuple{
        public SkillEffect effect;
        public float value1;
        public float value2;
    }

    [System.Serializable]
    public struct DialogueEntry{
        public Quest quest;
        public string eventFlag;
        public Fog.Dialogue.Dialogue dialogue;
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar DialogueEntry no editor da unity
    [CustomPropertyDrawer(typeof(DialogueEntry))]
    public class DialogueEntryDrawer : PropertyDrawer{

        private SerializedProperty quest, eventFlag, dialogue;
        private int index;
        private Rect questRect;
        private Rect eventRect;
        private Rect dialogueRect; 

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null)? 2 : (_quest.objectReferenceValue == null)? 2 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            dialogue = property.FindPropertyRelative("dialogue");
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null)? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // A posicao do campo de dialogue e relativa a posicao do campo de eventFlag
            dialogueRect = new Rect(new Vector2(eventRect.x, eventRect.y + eventRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if(quest.objectReferenceValue != null){
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = new string[_quest.events.Keys.Count];
                _quest.events.Keys.CopyTo(keys, 0);
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length-1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0)? keys[index] : "";
            }else{
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }
            // Cria o campo de referencia de dialogue
            EditorGUI.PropertyField(dialogueRect, dialogue);

            EditorGUI.EndProperty();
        }
    }
    #endif

    [System.Serializable]
    public struct QuestEvent{
        public Quest quest;
        public string eventFlag;
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomPropertyDrawer(typeof(QuestEvent))]
    public class QuestEventDrawer : PropertyDrawer{

        private SerializedProperty quest, eventFlag;
        private int index;
        private Rect questRect;
        private Rect eventRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null)? 1 : (_quest.objectReferenceValue == null)? 1 : 2;
            return (i * EditorGUIUtility.singleLineHeight) + 5f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null)? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if(quest.objectReferenceValue != null){
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = new string[_quest.events.Keys.Count];
                _quest.events.Keys.CopyTo(keys, 0);
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length-1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0)? keys[index] : "";
            }else{
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }

            EditorGUI.EndProperty();
        }
    }
    #endif
}