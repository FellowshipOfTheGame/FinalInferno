using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public abstract class Quest : ScriptableObject, IDatabaseItem {
        public const string IsActiveFlagString = "IsQuestActive";
        [SerializeField] protected string serializedID;
        public string SerializedID => serializedID;
        [SerializeField] protected bool active;
        public bool IsActive => active;
        public bool IsComplete {
            get {
                if (!IsActive)
                    return false;
                foreach (bool flag in events.Values) {
                    if (!flag)
                        return false;
                }
                return true;
            }
        }
        [SerializeField] protected int expReward = 0;
        // O número máximo de eventos permitidos é 62 por medida de segurança
        [SerializeField] protected QuestDictionary events = new QuestDictionary();

        public void LoadTables() {
            ResetQuest();
        }

        public void Preload() {
            ResetQuest();
        }

        public static bool IsConditionSatisfied(Quest quest, string eventFlag) {
            if (!quest)
                return true;
            if (eventFlag == IsActiveFlagString)
                return quest.IsActive;
            return quest.GetFlag(eventFlag);
        }

        public string[] GetSerializableFlagNames() {
            string[] keys = new string[events.Keys.Count];
            events.Keys.CopyTo(keys, 0);
            return keys;
        }

        public string[] GetAllFlagNames() {
            string[] keys = new string[events.Keys.Count + 1];
            keys[0] = IsActiveFlagString;
            events.Keys.CopyTo(keys, 1);
            return keys;
        }

        public int EventCount => events.Count;

        public bool GetFlag(string eventName) {
            return events.ContainsKey(eventName) && events[eventName];
        }

        public void SetFlag(string eventName, bool value) {
            if (IsActive && events.ContainsKey(eventName))
                events[eventName] = value;
        }

        public virtual void ResetQuest() {
            List<string> keyList = new List<string>(events.Keys);
            foreach (string key in keyList) {
                events[key] = false;
            }
            active = false;
        }

        public virtual void StartQuest() {
            ResetQuest();
            expReward = Mathf.Max(expReward, 0);
            active = true;
        }

        public virtual void CompleteQuest() {
            if (!IsActive || IsComplete)
                return;
            foreach (string key in events.Keys) {
                events[key] = true;
            }
            Party.Instance.GiveExp(expReward);
        }
    }
}
