using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class SaveLoader{
        public SaveInfo[] saves = new SaveInfo[5];
        //public SaveInfo save;

        public void Save(int slot){
            /*if(slot < 0 && slot > 4){
                Debug.Log("Slot de Save inválido");
                return;
            }

            List<SkillInfo> lsi = new List<SkillInfo>();
            SkillInfo si = new SkillInfo();

            saves[slot].xpParty = Party.Instance.xp;
            for(int i = 0; i < 4; i++){
                saves[slot].archetype.Insert(Party.Instance.characters[i].archetype.name);
                saves[slot].hpCur.Insert(Party.Instance.characters[i].hpCur);
                saves[slot].position.Insert(Party.Instance.characters[i].position);
                
                foreach (PlayerSkill skill in Party.Instance.characters[i].archetype.skills){
                    si.xp = skill.xp;
                    si.active = skill.active;
                    lsi.Insert(si);
                }
                saves[slot].skills.Insert(lsi);
            }*/
        }

        public void Load(int slot){
            /*for(int i = 0; i < 4; i++){
                Party.Instance.characters[i].archetype.name = saves[slot].archetype[i];
                Party.Instance.characters[i].hpCur = saves[slot].hpCur[i];
                Party.Instance.characters[i].position = saves[slot].position[i];
                
                foreach (SkillInfo skill in saves[slot].skills[i]){
                    Party.Instance.characters[i].archetype.skills[i].xp = skill.xp;
                    Party.Instance.characters[i].archetype.skills[i].active = skill.active;
                }
            }

            Party.Instance.GiveExp(saves[slot].xpParty);
        */}

        public void NewGame(){
            Party.Instance.GiveExp(0);
            
        }
    }
}