using UnityEngine;

namespace FinalInferno {
    public static class SaveLoader {
        private const string fileName = "SaveFile";
        private static DataSaver<SaveFile> dataSaver = new DataSaver<SaveFile>(fileName, true);
        private static SaveFile saveFile = dataSaver.LoadData();
        public static int SaveSlot { get => saveFile.Slot; set => saveFile.Slot = value; }
        public static bool AutoSave {
            get => saveFile != null && PlayerPrefs.GetString("autosave", "true") == "true";
            set {
                if (saveFile != null) {
                    PlayerPrefs.SetString("autosave", (value ? "true" : "false"));
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
            Quest mainQuest = AssetManager.LoadAsset<Quest>("MainQuest");
            mainQuest.StartQuest(true);
            SceneLoader.LoadCustsceneFromMenu(StaticReferences.FirstScene, StaticReferences.FirstDialogue);
        }

        public static void StartDemo() {
            ResetGame();
            Quest mainQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
            mainQuest.StartQuest(true);
            mainQuest = AssetManager.LoadAsset<Quest>("MainQuest");
            mainQuest.StartQuest(true);
            SceneLoader.LoadOWSceneWithDefaultPositions("Demo");
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