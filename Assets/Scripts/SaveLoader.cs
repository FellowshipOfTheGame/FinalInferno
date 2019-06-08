using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class SaveLoader{
        public SaveInfo[] saves = new SaveInfo[5];
        //public SaveInfo save;

        /*public void Save(int slot){
            if(slot < 0 && slot > 4){
                Debug.Log("Slot de Save inválido");
                return;
            }

            Party instance = Party.instance;
            List<SkillInfo> lsi = new List<SkillInfo>();
            SkillInfo si = new SkillInfo();

            save[slot].xpParty = instance.xp;
            for(int i = 0; i < 4; i++){
                save[slot].archetype.Insert(instance.character[i].archetype.name);
                save[slot].hpCur.Insert(instance.character[i].hpCur);
                save[slot].position.Insert(instance.character[i].position);
                
                foreach (PlayerSkill skill in instance.character[i].archetype.skills){
                    si.xp = skill.xp;
                    si.active = skill.active;
                    lsi.Insert(si);
                }
                save[slot].skills.Insert(lsi);
            }
        }
        
        public void Load(int slot){
            Party.instance.xp = save[slot].xpParty;
            for(int i = 0; i < 4; i++){
                Party.instance.character[i].archetype.name = save[slot].archetype[i];
                Party.instance.character[i].hpCur = save[slot].hpCur[i];
                Party.instance.character[i].position = save[slot].position[i];
                
                foreach (SkillInfo skill in saves[slot].skills[i]){
                    Party.instance.character[i].archetype.skills[i].xp = skill.xp;
                    Party.instance.character[i].archetype.skills[i].active = skill.active;
                }
            }
        }
        */

        public void NewGame(){
            Party.Instance.GiveExp(0);
            
        }
    }
}