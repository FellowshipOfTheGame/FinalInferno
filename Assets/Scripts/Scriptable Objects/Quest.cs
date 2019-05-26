using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
    public class Quest : ScriptableObject
    {
        public bool active;
        [SerializeField]
        public QuestDictionary events;
        public virtual void StartQuest(){
            foreach(string key in events.Keys){
                if(key == "Active")
                    events[key] = true;
                else
                    events[key] = false;
            }
            active = true;
        }
        public virtual void CompleteQuest(){
            foreach(string key in events.Keys){
                events[key] = true;
            }
            active = false;
        }
    }
}
