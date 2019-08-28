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
        // TO DO: Valor padrão deveria ser false, mas n tem menu de salvar o jogo
        [SerializeField] public bool autoSave = true;
        // Uma array de saves inicializados com valores padrão
        public SaveInfo[] saves = new SaveInfo[nSaveSlots];

        public SavePreviewInfo Preview(int slotNumber){
            slotNumber = Mathf.Clamp(slotNumber, 0, nSaveSlots);
            return new SavePreviewInfo(saves[slotNumber]);
        }

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
            List<Quest> quests = new List<Quest>(Party.Instance.activeQuests.ToArray());
            saves[Slot].quest = new QuestInfo[quests.Count];

            // Salva as informações de cada personagem no slot atual
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                saves[Slot].archetype[i] = Party.Instance.characters[i].archetype.name;
                saves[Slot].hpCur[i] = Party.Instance.characters[i].hpCur;
                saves[Slot].position[i] = Party.Instance.characters[i].position;
                saves[Slot].heroSkills[i].skills = new SkillInfo[Party.Instance.characters[i].archetype.skills.Count];
                
                for (int j = 0; j < Party.Instance.characters[i].archetype.skills.Count; j++){
                    saves[Slot].heroSkills[i].skills[j] = new SkillInfo((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]);
                }
            }

            // Salva as informações de todas as quests ativas
            for(int i = 0; i < quests.Count; i++){
                QuestInfo qinfo;
                qinfo.name = quests[i].name;
                qinfo.flagsNames = new string[quests[i].events.Keys.Count];

                quests[i].events.Keys.CopyTo(qinfo.flagsNames, 0);
                System.Array.Sort(qinfo.flagsNames);

                qinfo.flagsTrue = 0;
                ulong bitValue = 1;
                for(int j = 0; j < quests[i].events.Count; j++){
                    qinfo.flagsTrue = qinfo.flagsTrue | (bitValue * ((quests[i].events[qinfo.flagsNames[j]])? (ulong)1 : (ulong)0));
                    bitValue =  bitValue << 1;
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
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).GiveExp(saves[Slot].heroSkills[i].skills[j].xpCumulative);
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).active = saves[Slot].heroSkills[i].skills[j].active;
                }
            }

            Party.Instance.activeQuests.Clear();
            foreach(QuestInfo questInfo in saves[Slot].quest){
                Quest quest = AssetManager.LoadAsset<Quest>(questInfo.name);
                ulong bitValue = 1;
                for(int i = 0; i < quest.events.Count; i++){
                    quest.events[questInfo.flagsNames[i]] = (questInfo.flagsTrue & bitValue) != 0;
                    bitValue =  bitValue << 1;
                }
                Party.Instance.activeQuests.Add(quest);
            }

            Party.Instance.currentMap = saves[Slot].mapName;
            Party.Instance.GiveExp(saves[Slot].xpParty);
        }
    }
}
