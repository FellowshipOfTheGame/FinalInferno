using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public bool HasCheckPoint{
            get{
                return (saves[Slot].mapName != null && saves[Slot].mapName != "");
            }
        }
        // Uma array de saves inicializados com valores padrão
        public SaveInfo[] saves = new SaveInfo[nSaveSlots];

        public SaveFile(){
            slot = 0;
            saves = new SaveInfo[nSaveSlots];
            for(int i = 0; i < nSaveSlots; i++){
                saves[i] = new SaveInfo();
                // O valor padrão de autosave precisa ser diferente do valor default de bool
                saves[i].autoSave = true;
            }
        }

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

        public bool Equals(SaveFile otherFile){
            for(int i = 0; i < NSaveSlots; i++){
                if(!saves[i].Equals(otherFile.saves[i]))
                    return false;
            }
            return true;
        }

        public void Save(){
            saves[Slot].version = Application.version;

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

            // Salva as informações do bestiario
            ReadOnlyDictionary<Enemy, int> bestiary = Party.Instance.Bestiary;
            Enemy[] enemies = new Enemy[bestiary.Count];
            bestiary.Keys.CopyTo(enemies, 0);

            saves[Slot].bestiary = new BestiaryEntry[bestiary.Count];

            for(int i = 0; i < bestiary.Count; i++){
                saves[Slot].bestiary[i] = new BestiaryEntry(enemies[i], bestiary[enemies[i]]);
            }

            // Salva as informações de configurações
            saves[Slot].volumeInfo = StaticReferences.VolumeController.GetInfo();
            saves[Slot].autoSave = SaveLoader.AutoSave;
        }

        private bool IsOlder(string file, string compare){
            if(file == null || file == "") return true;

            string[] numbers = file.Split('.');
            if(numbers.Length < 3){
                Debug.LogError("Save file version number is missing components");
                return true;
            }

            string[] numbersCompare = compare.Split('.');
            if(numbersCompare.Length < 3){
                Debug.LogError("Compare version number is missing components");
                return true;
            }

            int chapter;
            int major;
            int minor;
            int chapterCompare;
            int majorCompare;
            int minorCompare;
            try{
                chapter = int.Parse(numbers[0]);
                major = int.Parse(numbers[1]);
                minor = int.Parse(numbers[2]);
                chapterCompare = int.Parse(numbersCompare[0]);
                majorCompare = int.Parse(numbersCompare[1]);
                minorCompare = int.Parse(numbersCompare[2]);
            }catch(System.Exception e){
                Debug.LogError("Error parsing save version number");
                return true;
            }

            if(chapter > chapterCompare){
                return false;
            }else if(chapter < chapterCompare){
                return true;
            }else{
                if(major > majorCompare){
                    return false;
                }else if(major < majorCompare){
                    return true;
                }else{
                    if(minor >= minorCompare){
                        return false;
                    }else{
                        return true;
                    }
                }
            }
        }

        public bool IsSlotEmpty(int slot){
            // Qualquer jogo salvo tera exp, pois no minimo a exp da primeira batalha foi dada
            if(saves[slot].xpParty <= 0)
                return true;
            return false;
        }

        private void UpdateSlot(){
            if(!IsSlotEmpty(Slot) && saves[Slot].version != null && saves[Slot].version != ""){
                // Da versão 1.6.6 pra trás a informação de autosave não existia e o valor padrão viria false
                // mas o valor padrão precisa ser corrigido para true nesses casos
                if(IsOlder("1.6.6", Application.version) && IsOlder(saves[Slot].version, "1.6.7")){
                    Debug.Log($"Setting autosave to True, previous value was {saves[Slot].autoSave}");
                    saves[Slot].autoSave = true;
                }
            }
        }

        public void Load(){
            // Verifica se há alguma incompatibilidade entre a versão do jogo do save armazenado e versão atual
            UpdateSlot();

            Party.Instance.GiveExp(saves[Slot].xpParty);
            Party.Instance.currentMap = saves[Slot].mapName;

            // Carrega as informações de cada personagem
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                Party.Instance.characters[i].archetype = AssetManager.LoadAsset<Hero>(saves[Slot].archetype[i]);
                Party.Instance.characters[i].hpCur = saves[Slot].hpCur[i];
                Party.Instance.characters[i].position = saves[Slot].position[i];
                
                for (int j = 0; j < saves[Slot].heroSkills[i].skills.Length; j++){//SkillInfo skill in saves[Slot].skills[i]){
                    if(saves[Slot].heroSkills[i].skills[j].xpCumulative > 0){
                        ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).GiveExp(saves[Slot].heroSkills[i].skills[j].xpCumulative);
                    }
                    ((PlayerSkill)Party.Instance.characters[i].archetype.skills[j]).active = saves[Slot].heroSkills[i].skills[j].active;
                }

                Party.Instance.characters[i].archetype.skillsToUpdate.Clear();
                foreach(PlayerSkill skill in Party.Instance.characters[i].archetype.skills){
                    if(skill.Level > 0){
                        Party.Instance.characters[i].archetype.skillsToUpdate.Add(skill);
                    }
                }
                Party.Instance.characters[i].archetype.UnlockSkills();
            }

            // Carrega as informações das quests em progresso
            Party.Instance.activeQuests.Clear();
            foreach(QuestInfo questInfo in saves[Slot].quest){
                Quest quest = AssetManager.LoadAsset<Quest>(questInfo.name);
                quest.StartQuest(true);
                ulong bitValue = 1;
                for(int i = 0; i < quest.events.Count; i++){
                    quest.events[questInfo.flagsNames[i]] = (questInfo.flagsTrue & bitValue) != 0;
                    bitValue =  bitValue << 1;
                }
            }

            // Carrega as informações do bestiario
            Party.Instance.ReloadBestiary(saves[Slot].bestiary);

            // Carrega as informações de configuração
            StaticReferences.VolumeController.ResetValues(saves[Slot].volumeInfo ?? new VolumeController.VolumeInfo());
            SaveLoader.AutoSave = saves[Slot].autoSave;
        }
    }
}
