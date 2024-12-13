using System;
using System.Collections.ObjectModel;
using FinalInferno.CustomExtensions;
using UnityEngine;

namespace FinalInferno {
    [Serializable]
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
            saves[Slot].quest = Party.Instance.GetActiveQuestInfo();
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
            LoadPartyInfo();
            LoadQuestsInfo();
            LoadBestiaryInfo();
            LoadSettings();
        }

        public bool HasNewerSaveSlot() {
            for (int index = 0; index < nSaveSlots; index++) {
                if (IsSlotEmpty(index) || !SlotHasVersionString(index))
                    continue;
                if (Application.version.IsOlderVersionThan(saves[index].version))
                    return true;
            }
            return false;
        }

        public bool IsSlotEmpty(int slot) {
            // Qualquer jogo salvo tera exp, pois no minimo a exp da primeira batalha foi dada
            return saves[slot].xpParty <= 0;
        }

        private bool SlotHasVersionString(int index) {
            return !string.IsNullOrEmpty(saves[index].version);
        }

        public bool HasOlderSaveSlot() {
            for (int index = 0; index < nSaveSlots; index++) {
                if (IsSlotEmpty(index))
                    continue;
                if (!SlotHasVersionString(index) || Application.version.IsNewerVersionThan(saves[index].version))
                    return true;
            }
            return false;
        }

        public void ApplyVersionUpdates() {
            for (int index = 0; index < nSaveSlots; index++) {
                if (IsSlotEmpty(index))
                    continue;
                if (SlotHasVersionString(index)) {
                    UpdateAutoSaveSettings167(index);
                    UpdateMainQuestProgress200(index);
                }
                saves[index].version = Application.version;
            }
        }

        private void UpdateAutoSaveSettings167(int index) {
            if (!Application.version.IsNewerVersionThan("1.6.6") || !saves[index].version.IsOlderVersionThan("1.6.7"))
                return;
            Debug.Log($"Setting autosave to True, previous value was {saves[index].autoSave}");
            saves[index].autoSave = true;
        }

        private void UpdateMainQuestProgress200(int index) {
            if (!Application.version.IsNewerVersionThan("1.7.0") || !saves[index].version.IsOlderVersionThan("2.0.0"))
                return;
            Debug.Log($"Applying Main Quest updates for slot {index}");
            for (int i = 0; i < saves[index].quest.Length; i++) {
                if (saves[index].quest[i].name != "MainQuest")
                    continue;

                int previousFlagCount = saves[index].quest[i].flagsNames.Length;
                int defaultFlagIndex = RemoveDefaultFlagName200(index, i);
                ulong CerberusDeadFlagBit = FindCerberusDeadFlagBit200(index, i);
                if ((CerberusDeadFlagBit & saves[index].quest[i].flagsTrue) != 0) {
                    saves[index].quest[i].flagsTrue = 0;
                    ulong bitValue = 1;
                    bitValue <<= saves[index].quest[i].flagsNames.Length - 1;
                    while (bitValue > 0) {
                        saves[index].quest[i].flagsTrue |= bitValue;
                        bitValue >>= 1;
                    }
                } else {
                    ulong previousFlags = saves[index].quest[i].flagsTrue;
                    saves[index].quest[i].flagsTrue = 0;
                    ulong copyBitMask = 1;
                    ulong pasteBitMask = 1;
                    for (int flag = 0; flag < previousFlagCount; flag++) {
                        if (flag == defaultFlagIndex) {
                            copyBitMask <<= 1;
                            continue;
                        }
                        if ((previousFlags & copyBitMask) != 0)
                            saves[index].quest[i].flagsTrue |= pasteBitMask;
                        copyBitMask <<= 1;
                        pasteBitMask <<= 1;
                    }
                }
                return;
            }
        }

        private int RemoveDefaultFlagName200(int slot, int questIndex) {
            bool skipped = false;
            int defaultFlagIndex = -1;
            string[] previousList = (string[])saves[slot].quest[questIndex].flagsNames.Clone();
            saves[slot].quest[questIndex].flagsNames = new string[previousList.Length - 1];
            for (int i = 0; i < previousList.Length; i++) {
                if (previousList[i] == "Default") {
                    skipped = true;
                    defaultFlagIndex = i;
                    continue;
                }
                saves[slot].quest[questIndex].flagsNames[skipped ? i - 1 : i] = previousList[i];
            }
            return defaultFlagIndex;
        }

        private ulong FindCerberusDeadFlagBit200(int slot, int questIndex) {
            for (int i = 0; i < saves[slot].quest[questIndex].flagsNames.Length; i++) {
                if (saves[slot].quest[questIndex].flagsNames[i] == "CerberusDead")
                    return (ulong)1 << i;
            }
            throw new Exception("[SaveFile]: Could not find CerberusDead flag in save file");
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
            Party.Instance.LoadQuestProgress(saves[Slot].quest);
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
