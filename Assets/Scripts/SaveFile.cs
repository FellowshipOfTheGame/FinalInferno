using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinalInferno.CustomExtensions;
using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class SaveFile {
        private const int nSaveSlots = 5;
        public static int NSaveSlots => nSaveSlots;
        [SerializeField] private int slot = 0;
        public int Slot {
            get => slot;
            set {
                if (value >= 0 && value < nSaveSlots) {
                    slot = value;
                }
            }
        }
        public bool HasCheckPoint => !string.IsNullOrEmpty(saves[Slot].mapName);
        public SaveInfo[] saves = new SaveInfo[nSaveSlots];

        public SaveFile() {
            slot = 0;
            saves = new SaveInfo[nSaveSlots];
            for (int i = 0; i < nSaveSlots; i++) {
                saves[i] = new SaveInfo();
                // O valor padrão de autosave precisa ser diferente do valor default de bool
                saves[i].autoSave = true;
            }
        }

        public SavePreviewInfo PreviewSingleSlot(int slotNumber) {
            slotNumber = Mathf.Clamp(slotNumber, 0, nSaveSlots);
            return new SavePreviewInfo(saves[slotNumber]);
        }

        public SavePreviewInfo[] PreviewAllSlots() {
            SavePreviewInfo[] previews = new SavePreviewInfo[nSaveSlots];
            for (int i = 0; i < nSaveSlots; i++) {
                previews[i] = new SavePreviewInfo(saves[i]);
            }
            return previews;
        }

        public bool Equals(SaveFile otherFile) {
            for (int i = 0; i < NSaveSlots; i++) {
                if (!saves[i].Equals(otherFile.saves[i])) {
                    return false;
                }
            }
            return true;
        }

        public void Save() {
            saves[Slot].version = Application.version;
            SavePartyInfo();
            SaveQuestsInfo();
            SaveBestiaryInfo();
            SaveSettings();
        }

        private void SavePartyInfo() {
            saves[Slot].xpParty = Party.Instance.XpCumulative;
            saves[Slot].mapName = Party.Instance.currentMap;
            saves[Slot].archetype = new string[Party.Instance.characters.Count];
            saves[Slot].hpCur = new int[Party.Instance.characters.Count];
            saves[Slot].position = new Vector2[Party.Instance.characters.Count];
            saves[Slot].heroSkills = new SkillInfoArray[Party.Instance.characters.Count];
            for (int i = 0; i < Party.Instance.characters.Count; i++) {
                SaveCharacterInfo(i);
            }
        }

        private void SaveCharacterInfo(int index) {
            Character character = Party.Instance.characters[index];
            saves[Slot].archetype[index] = character.archetype.name;
            saves[Slot].hpCur[index] = character.hpCur;
            saves[Slot].position[index] = character.position;
            saves[Slot].heroSkills[index].skills = new SkillInfo[character.archetype.skills.Count];

            for (int skillIndex = 0; skillIndex < character.archetype.skills.Count; skillIndex++) {
                saves[Slot].heroSkills[index].skills[skillIndex] = new SkillInfo((PlayerSkill)character.archetype.skills[skillIndex]);
            }
        }

        private void SaveQuestsInfo() {
            List<Quest> questList = new List<Quest>(Party.Instance.activeQuests.ToArray());
            saves[Slot].quest = new QuestInfo[questList.Count];
            for (int i = 0; i < questList.Count; i++) {
                Quest quest = questList[i];
                QuestInfo qinfo = new QuestInfo();

                qinfo.name = quest.name;
                qinfo.flagsNames = quest.FlagNames;
                System.Array.Sort(qinfo.flagsNames);
                qinfo.SetQuestFlags(quest);

                saves[Slot].quest[i] = qinfo;
            }
        }

        private void SaveBestiaryInfo() {
            ReadOnlyDictionary<Enemy, int> bestiary = Party.Instance.Bestiary;
            Enemy[] enemies = new Enemy[bestiary.Count];
            bestiary.Keys.CopyTo(enemies, 0);

            saves[Slot].bestiary = new BestiaryEntry[bestiary.Count];

            for (int i = 0; i < bestiary.Count; i++) {
                saves[Slot].bestiary[i] = new BestiaryEntry(enemies[i], bestiary[enemies[i]]);
            }
        }

        private void SaveSettings() {
            saves[Slot].volumeInfo = StaticReferences.VolumeController.GetInfo();
            saves[Slot].autoSave = SaveLoader.AutoSave;
        }

        public void Load() {
            // Verifica se há alguma incompatibilidade entre a versão do jogo do save armazenado e versão atual
            ApplyVersionUpdates();
            LoadPartyInfo();
            LoadQuestsInfo();
            LoadBestiaryInfo();
            LoadSettings();
        }

        private void ApplyVersionUpdates() {
            if (!IsSlotEmpty(Slot) && saves[Slot].version != null && saves[Slot].version != "") {
                if (Application.version.IsNewerVersionThan("1.6.6") && saves[Slot].version.IsOlderVersionThan("1.6.7")) {
                    Debug.Log($"Setting autosave to True, previous value was {saves[Slot].autoSave}");
                    saves[Slot].autoSave = true;
                }
            }
        }

        public bool IsSlotEmpty(int slot) {
            // Qualquer jogo salvo tera exp, pois no minimo a exp da primeira batalha foi dada
            return saves[slot].xpParty <= 0;
        }

        private void LoadPartyInfo() {
            Party.Instance.GiveExp(saves[Slot].xpParty);
            Party.Instance.currentMap = saves[Slot].mapName;

            for (int i = 0; i < Party.Instance.characters.Count; i++) {
                LoadCharacterInfo(i);
            }
        }

        private void LoadCharacterInfo(int index) {
            Character character = Party.Instance.characters[index];
            character.archetype = AssetManager.LoadAsset<Hero>(saves[Slot].archetype[index]);
            character.hpCur = saves[Slot].hpCur[index];
            character.position = saves[Slot].position[index];
            SkillInfoArray skillInfoArray = saves[Slot].heroSkills[index];

            LoadCharacterSkillExp(character, skillInfoArray);
            LoadCharacterSkillTree(character);
        }

        private static void LoadCharacterSkillExp(Character character, SkillInfoArray skillInfoArray) {
            for (int j = 0; j < skillInfoArray.skills.Length; j++) {
                if (skillInfoArray.skills[j].xpCumulative > 0) {
                    ((PlayerSkill)character.archetype.skills[j]).GiveExp(skillInfoArray.skills[j].xpCumulative);
                }
                ((PlayerSkill)character.archetype.skills[j]).active = skillInfoArray.skills[j].active;
            }
        }

        private static void LoadCharacterSkillTree(Character character) {
            character.archetype.skillsToUpdate.Clear();
            foreach (PlayerSkill skill in character.archetype.skills) {
                if (skill.Level > 0) {
                    character.archetype.skillsToUpdate.Add(skill);
                }
            }
            character.archetype.UnlockSkills();
        }

        private void LoadQuestsInfo() {
            Party.Instance.activeQuests.Clear();
            foreach (QuestInfo questInfo in saves[Slot].quest) {
                Quest quest = AssetManager.LoadAsset<Quest>(questInfo.name);
                quest.StartQuest();
                ulong bitValue = 1;
                for (int i = 0; i < quest.EventCount; i++) {
                    quest.SetFlag(questInfo.flagsNames[i], (questInfo.flagsTrue & bitValue) != 0);
                    bitValue = bitValue << 1;
                }
            }
        }

        private void LoadBestiaryInfo() {
            Party.Instance.ReloadBestiary(saves[Slot].bestiary);
        }

        private void LoadSettings() {
            StaticReferences.VolumeController.ResetValues(saves[Slot].volumeInfo ?? new VolumeInfo());
            SaveLoader.AutoSave = saves[Slot].autoSave;
        }
    }
}
