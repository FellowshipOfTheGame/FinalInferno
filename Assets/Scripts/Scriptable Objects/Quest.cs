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
        public Quest PartyReference{
            get{
                return Party.Instance.activeQuests.Find(quest => quest.name == name);
            }
        }
        public Quest StaticReference{
            get{
                return StaticReferences.instance.activeQuests.Find(quest => quest.name == name);
            }
        }
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
            if((PartyReference == null) && (StaticReference == null)){
                ResetQuest();
                Party.Instance.activeQuests.Add(this);
                StaticReferences.instance.activeQuests.Add(this);
            }else{
                Debug.Log("Quest has already begun");
            }
        }
        public virtual void CompleteQuest(){
            foreach(string key in events.Keys){
                events[key] = true;
            }
            active = false;
            Party.Instance.activeQuests.Remove(PartyReference);
            StaticReferences.instance.activeQuests.Remove(StaticReference);
        }
    }
}
