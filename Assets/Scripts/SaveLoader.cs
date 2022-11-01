using UnityEngine;

namespace FinalInferno {
    public static class SaveLoader {
        private const string fileName = "SaveFile";
        private const string autosavePlayerPrefKey = "autosave";
        private const string trueString = "true";
        private const string falseString = "false";
        private static DataSaver<SaveFile> dataSaver = new DataSaver<SaveFile>(fileName, true);
        private static SaveFile saveFile = InitSaveFile();
        public static int SaveSlot { get => saveFile.Slot; set => saveFile.Slot = value; }
        public static bool AutoSave {
            get => saveFile != null && PlayerPrefs.GetString(autosavePlayerPrefKey, trueString) == trueString;
            set {
                if (saveFile != null) {
                    PlayerPrefs.SetString(autosavePlayerPrefKey, (value ? trueString : falseString));
                }
            }
        }
        public static bool CanSaveGame {
            get {
                int charCount = 0;
                foreach (Character character in Party.Instance.characters) {
                    if (character.OverworldInstance != null) {
                        charCount++;
                    }
                }
                return (charCount == Party.Capacity);
            }
        }

        private static SaveFile InitSaveFile() {
            SaveFile loadedData = dataSaver.LoadData();
            if (loadedData.HasNewerSaveSlot()) {
                string backupSuffix = $"-{Application.version}({System.DateTime.Now})";
                dataSaver.CreateBackup(backupSuffix);
                loadedData = dataSaver.LoadData();
            } else if (loadedData.HasOlderSaveSlot()) {
                loadedData.ApplyVersionUpdates();
                dataSaver.SaveData(loadedData);
            }
            return loadedData;
        }

        public static SavePreviewInfo[] PreviewAllSlots() {
            return saveFile.PreviewAllSlots();
        }

        public static SavePreviewInfo PreviewSingleSlot(int slotNumber) {
            return saveFile.PreviewSingleSlot(slotNumber);
        }

        public static void SaveGame() {
            if (!CanSaveGame) {
                Debug.Log("Attempted to save the game when it shouldn't be possible");
                return;
            }
            Debug.Log("Saving game...");
            saveFile.Save();
            dataSaver.SaveData(saveFile);
            Debug.Log("Game saved (...probably)");
        }

        public static void LoadGame() {
            if (!saveFile.HasCheckPoint) {
                Debug.Log("Tried to load empty save file, starting new game instead...");
                NewGame();
                return;
            }
            ResetGame();
            saveFile.Load();
            SceneLoader.LoadOWSceneAndPositions(Party.Instance.currentMap);
        }

        public static void NewGame() {
            ResetGame();
            SceneLoader.LoadCustsceneFromMenu(StaticReferences.FirstScene, StaticReferences.FirstDialogue);
        }

        public static void StartDemo() {
            ResetGame();
            Party.Instance.StartQuest(StaticReferences.DemoQuest);
            SceneLoader.LoadOWSceneWithDefaultPositions(StaticReferences.DemoScene);
        }

        public static void ResetGame() {
            Party.Instance.ResetParty();
            ResetSettings();
        }

        private static void ResetSettings() {
            AutoSave = true;
            StaticReferences.VolumeController.ResetValues();
        }
    }
}