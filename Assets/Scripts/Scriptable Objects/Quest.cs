﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
    public class Quest : ScriptableObject, IDatabaseItem
    {
        public bool active;
        // O número máximo de eventos permitidos é 62 por medida de segurança
        public QuestDictionary events;

        public void LoadTables(){
            ResetQuest();
        }

        public void Preload(){
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
            active = false;
        }

        public virtual void StartQuest(bool forceReset = false){
            if(!active || forceReset){
                ResetQuest();
                Party.Instance.activeQuests.Add(this);
                active = true;
            }else{
                Debug.Log("Quest has already begun");
            }
        }

        public virtual void CompleteQuest(){
            foreach(string key in events.Keys){
                events[key] = true;
            }
            active = false;
            // TO DO: Quest não repetitiveis não devem ser removidas daqui
            Party.Instance.activeQuests.Remove(this);
        }
    }
}
