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
        Self
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

    [System.Serializable]
    public class QuestDictionary : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<string, bool>{ }

    [System.Serializable]
    public struct DialogueEntry{
        public Quest quest;
        public string eventFlag;
        public Fog.Dialogue.Dialogue dialogue;
    }
    [CustomPropertyDrawer(typeof(DialogueEntry))]
    public class DialogueEntryDrawer : PropertyDrawer{

        public ScriptableObject questObj = null;
        public ScriptableObject dialogueObj = null;
        public int index;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            int i = (questObj == null)? 2 : 3;
            return i * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

            Rect questRect = new Rect(position.position, new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            Rect eventRect;
            Rect dialogueRect; 
            //position.size = questRect.size + eventRect.size + dialogueRect.size;

            questObj = (ScriptableObject)EditorGUI.ObjectField(questRect, "Quest", questObj, typeof(Quest), false);
            if(questObj != null){
                eventRect = new Rect(new Vector2(questRect.x, questRect.y + questRect.height + 1), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
                dialogueRect = new Rect(new Vector2(eventRect.x, eventRect.y + eventRect.height + 1), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));

                Quest quest = (Quest)questObj;
                string[] keys = new string[quest.events.Keys.Count];
                quest.events.Keys.CopyTo(keys, 0);
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
            }else{
                dialogueRect = new Rect(new Vector2(questRect.x, questRect.y + questRect.height + 1), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));

                index = 0;
            }
            dialogueObj = (ScriptableObject)EditorGUI.ObjectField(dialogueRect, "Dialogue", dialogueObj, typeof(Fog.Dialogue.Dialogue), false);
        }
    }
}