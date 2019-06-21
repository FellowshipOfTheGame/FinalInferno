using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [System.Serializable]
    public class SaveFile{
        private const int nSaveSlots = 5;
        public static int NSaveSlots { get{ return nSaveSlots; } }
        [SerializeField] private int slot = 0;
        public int Slot{
            get{
                return slot;
            }
            set{
                if(value >= 0 && value < nSaveSlots)
                    slot = value;
            }
        }
        // Uma array de saves inicializados com valores padrão
        public SaveInfo[] saves = new SaveInfo[nSaveSlots];

        public SavePreviewInfo[] Preview(){
            SavePreviewInfo[] previews = new SavePreviewInfo[nSaveSlots];
            for(int i = 0; i < nSaveSlots; i++){
                previews[i] = new SavePreviewInfo(saves[i]);
            }
            return previews;
        }

        public void Save(){

            saves[Slot].xpParty = Party.Instance.XpCumulative;
            saves[Slot].mapName = Party.Instance.currentMap;
            saves[Slot].archetype = new string[Party.Instance.characters.Count];
            saves[Slot].hpCur = new int[Party.Instance.characters.Count];
            saves[Slot].position = new Vector2[Party.Instance.characters.Count];
            saves[Slot].heroSkills = new SkillInfoArray[Party.Instance.characters.Count];
            List<Quest> quests = AssetManager.LoadBundleAssets<Quest>();
            saves[Slot].quest = new QuestInfo[quests.Count];

            // Salva as informações de cada personagem no slot atual
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                saves[Slot].archetype[i] = Party.Instance.characters[i].archetype.name;
                saves[Slot].hpCur[i] = Party.Instance.characters[i].hpCur;
                saves[Slot].position[i] = Party.Instance.characters[i].position;
                saves[Slot].heroSkills[i].skills = new SkillInfo[Party.Instance.characters[i].archetype.skills.Count];
                
                for (int j = 0; j < Party.Instance.characters[i].archetype.skills.Count; j++){
                    saves[Slot].heroSkills[i].skills[j].xp = ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).XpCumulative;
                    saves[Slot].heroSkills[i].skills[j].active = ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).active;
                }
            }

            // Salva as informações de todas as quests do jogo
            for(int i = 0; i < quests.Count; i++){
                QuestInfo qinfo;
                qinfo.name = quests[i].name;
                qinfo.flagsNames = new string[quests[i].events.Keys.Count];

                quests[i].events.Keys.CopyTo(qinfo.flagsNames, 0);
                System.Array.Sort(qinfo.flagsNames);

                qinfo.flagsTrue = 0;
                for(int j = 0; j < quests[i].events.Count; j++){
                    qinfo.flagsTrue = qinfo.flagsTrue & ((int)Mathf.Pow(2, j) * ((quests[i].events[qinfo.flagsNames[j]])? 1 : 0));
                }
                saves[Slot].quest[i] = qinfo;
            }
        }

        public void Load(){
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                Party.Instance.characters[i].archetype = AssetManager.LoadAsset<Hero>(saves[Slot].archetype[i]);
                Party.Instance.characters[i].hpCur = saves[Slot].hpCur[i];
                Party.Instance.characters[i].position = saves[Slot].position[i];
                
                for (int j = 0; j < saves[Slot].heroSkills[i].skills.Length; j++){//SkillInfo skill in saves[Slot].skills[i]){
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).GiveExp(saves[Slot].heroSkills[i].skills[j].xp);
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).active = saves[Slot].heroSkills[i].skills[j].active;
                }
            }

            foreach(QuestInfo questInfo in saves[Slot].quest){
                Quest quest = AssetManager.LoadAsset<Quest>(questInfo.name);
                for(int i = 0; i < quest.events.Count; i++){
                    quest.events[questInfo.flagsNames[i]] = (questInfo.flagsTrue & (int)Mathf.Pow(2, i)) == (int)Mathf.Pow(2, i);
                }
            }

            Party.Instance.currentMap = saves[Slot].mapName;
            Party.Instance.GiveExp(saves[Slot].xpParty);
        }
    }
}
