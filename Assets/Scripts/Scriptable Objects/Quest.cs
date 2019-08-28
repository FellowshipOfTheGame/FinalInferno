using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
    public class Quest : ScriptableObject
    {
        public bool active;
        // O número máximo de eventos permitidos é 62 por medida de segurança
        public QuestDictionary events;
        public void Awake(){
            ResetQuest();
        }
        private void ResetQuest(){
            List<string> keyList = new List<string>(events.Keys);
            foreach(string key in keyList){
                if(key == "Active" || key == "Default")
                    events[key] = true;
                else
                    events[key] = false;
            }
            active = true;
        }
        public virtual void StartQuest(){
            if(!Party.Instance.activeQuests.Contains(this)){
                ResetQuest();
                Party.Instance.activeQuests.Add(this);
            }
        }
        public virtual void CompleteQuest(){
            foreach(string key in events.Keys){
                events[key] = true;
            }
            active = false;
            Party.Instance.activeQuests.Remove(this);
        }
    }
}
