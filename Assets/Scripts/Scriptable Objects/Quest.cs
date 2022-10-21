using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    public abstract class Quest : ScriptableObject, IDatabaseItem {
        public const string IsActiveFlagString = "IsQuestActive";
        [SerializeField] protected string serializedID;
        [SerializeField] protected bool active;
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
                return quest.active;
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
            if (active && events.ContainsKey(eventName))
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
            Party.Instance.activeQuests.Remove(this);
            Party.Instance.activeQuests.Add(this);
            active = true;
        }

        public virtual void TryStartQuest() {
            if (!active) {
                StartQuest();
            } else {
                Debug.LogWarning("Quest has already begun", this);
            }
        }

        public virtual void CompleteQuest() {
            if (!active)
                return;
            foreach (string key in events.Keys) {
                events[key] = true;
            }
            active = false;
            Party.Instance.GiveExp(expReward);
        }
    }
}
