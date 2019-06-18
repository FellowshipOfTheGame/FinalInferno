using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [System.Serializable]
    public class SaveFile{
        private const int nSaveSlots = 5;
        public static int NSaveSlots { get{ return nSaveSlots; } }
        [SerializeField]
        private int slot = 0;
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
            List<SkillInfo> lsi = new List<SkillInfo>();
            SkillInfo si = new SkillInfo();

            saves[Slot].xpParty = Party.Instance.XpCumulative;
            saves[Slot].mapName = Party.Instance.currentMap;
            saves[Slot].archetype = new List<string>();
            saves[Slot].hpCur = new List<int>();
            saves[Slot].position = new List<Vector2>();
            saves[Slot].skills = new List<List<SkillInfo>>();
            saves[Slot].quest = new List<QuestInfo>();

            // Salva as informações de cada personagem no slot atual
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                saves[Slot].archetype.Add(Party.Instance.characters[i].archetype.name);
                saves[Slot].hpCur.Add(Party.Instance.characters[i].hpCur);
                saves[Slot].position.Add(Party.Instance.characters[i].position);
                
                foreach (PlayerSkill skill in Party.Instance.characters[i].archetype.skills){
                    si.xp = skill.XpCumulative;
                    si.active = skill.active;
                    lsi.Add(si);
                }
                saves[Slot].skills.Add(lsi);
            }

            // Salva as informações de todas as quests do jogo
            List<Quest> quests = AssetManager.LoadBundleAssets<Quest>();
            foreach(Quest quest in quests){
                QuestInfo qinfo;
                qinfo.name = quest.name;

                string[] keys = new string[quest.events.Keys.Count];
                quest.events.Keys.CopyTo(keys, 0);

                qinfo.flagsNames = new List<string>(keys);
                qinfo.flagsNames.Sort();

                qinfo.flagsTrue = 0;
                for(int i = 0; i < quest.events.Count; i++){
                    qinfo.flagsTrue = qinfo.flagsTrue & ((int)Mathf.Pow(2, i) * ((quest.events[qinfo.flagsNames[i]])? 1 : 0));
                }
                saves[Slot].quest.Add(qinfo);
            }
        }

        public void Load(){
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                Party.Instance.characters[i].archetype = AssetManager.LoadAsset<Hero>(saves[Slot].archetype[i]);
                Party.Instance.characters[i].hpCur = saves[Slot].hpCur[i];
                Party.Instance.characters[i].position = saves[Slot].position[i];
                
                foreach (SkillInfo skill in saves[Slot].skills[i]){
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[i]).GiveExp(skill.xp);
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[i]).active = skill.active;
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
