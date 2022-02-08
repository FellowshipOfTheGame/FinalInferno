using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
    public class Quest : ScriptableObject, IDatabaseItem {
        [SerializeField] private bool active;
        [SerializeField] private bool repeatable = false;
        [SerializeField] private int expReward = 0;
        // O número máximo de eventos permitidos é 62 por medida de segurança
        [SerializeField] private QuestDictionary events;

        public void LoadTables() {
            ResetQuest();
        }

        public void Preload() {
            ResetQuest();
        }

        public string[] FlagNames {
            get {
                string[] keys = new string[events.Keys.Count];
                events.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        public int EventCount => events.Count;

        public bool GetFlag(string eventName) {
            if (events.ContainsKey(eventName)) {
                return events[eventName];
            }
            return false;
        }

        public void SetFlag(string eventName, bool value) {
            if (active && events.ContainsKey(eventName)) {
                events[eventName] = value;
            }
        }

        public void ResetQuest() {
            List<string> keyList = new List<string>(events.Keys);
            foreach (string key in keyList) {
                if (key == "Default") {
                    events[key] = true;
                } else {
                    events[key] = false;
                }
            }
            active = false;
        }

        public virtual void StartQuest(bool forceReset = false) {
            if (!active || forceReset) {
                ResetQuest();
                expReward = Mathf.Max(expReward, 0);
                Party.Instance.activeQuests.Remove(this);
                Party.Instance.activeQuests.Add(this);
                active = true;
            } else {
                Debug.Log("Quest has already begun");
            }
        }

        public virtual void CompleteQuest() {
            if (active) {
                foreach (string key in events.Keys) {
                    events[key] = true;
                }
                active = false;
                Party.Instance.GiveExp(expReward);
                if (repeatable) {
                    Party.Instance.activeQuests.Remove(this);
                    ResetQuest();
                }
            }
        }
    }
}
