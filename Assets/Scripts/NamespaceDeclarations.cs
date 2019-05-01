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

    public enum EventFlag {
        TutorialOver,
        CerberusDefeated,
        MammonDefeated,
        Default
    }

    [System.Serializable]
    public class QuestDictionary : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<string, bool>{ }

    [System.Serializable]
    public struct DialogueEntry{
        Quest quest;
        string eventFlag;
        Fog.Dialogue.Dialogue dialogue;
    }
    [CustomEditor(typeof(DialogueEntry)), CanEditMultipleObjects]
    public class DialogueEntryGUI : Editor{
        SerializedProperty quest;
        SerializedProperty eventFlag;
        SerializedProperty dialogue;

        void OnEnable(){
            quest = serializedObject.FindProperty("quest");
            eventFlag = serializedObject.FindProperty("eventFlag");
            dialogue = serializedObject.FindProperty("dialogue");
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();
        }
    }
}