using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Fog.Dialogue;

namespace FinalInferno {
    public static class SceneLoader {
        private const string backgroundImageObjectName = "BackgroundImage";
        private const string mainMenuSceneName = "MainMenu";
        private const string BattleSceneName = "Battle";
        private static BattleInfo battleInfo;
        private static Dialogue cutsceneDialogue = null;
        public static Dialogue CutsceneDialogue {
            get => cutsceneDialogue;
            set {
                if (cutsceneDialogue == null) {
                    cutsceneDialogue = value;
                } else {
                    if (value == null) {
                        cutsceneDialogue = value;
                        Debug.LogWarning("Saved cutscene has been removed");
                    } else {
                        Debug.LogError("A cutscene has already been set");
                    }
                }
            }
        }
        public static UnityAction beforeSceneChange = null;
        public static UnityAction onSceneLoad = null;

        public static void LoadBattleScene(BattleInfoReference battleInfoReference) {
            RECalculator.encountersEnabled = false;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            Party.Instance.SaveOverworldPositions();
            battleInfo.CopyValues(battleInfoReference);
            SceneManager.sceneLoaded += OnBattleLoad;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(BattleSceneName);
        }

        private static void OverrideCharacterPositions(Vector2 newPosition) {
            foreach (Character character in Party.Instance.characters) {
                character.position = newPosition;
            }
        }

        #region LoadOWScene
        public static void LoadOWSceneAndPositions(string map) {
            Party.Instance.currentMap = map;
            if (SaveLoader.AutoSave) {
                SaveLoader.SaveGame();
            }

            SceneManager.sceneLoaded += MapAndPositionsLoaded;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }

        public static void LoadOWSceneWithSetPosition(string map, Vector2 newPosition) {
            OverrideCharacterPositions(newPosition);
            Party.Instance.currentMap = map;
            if (SaveLoader.AutoSave) {
                SaveLoader.SaveGame();
            }

            SceneManager.sceneLoaded += MapAndPositionsLoaded;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }

        public static void LoadOWSceneWithDefaultPositions(string map) {
            Party.Instance.currentMap = map;
            if (SaveLoader.AutoSave) {
                SaveLoader.SaveGame();
            }

            SceneManager.sceneLoaded += MapLoaded;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }

        public static void LoadOWSceneFromBattle(string map) {
            Party.Instance.currentMap = map;

            SceneManager.sceneLoaded += MapAndPositionsLoaded;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }
        #endregion

        #region LoadCutscene
        public static void LoadCustsceneFromMenu(string map, Fog.Dialogue.Dialogue dialogue) {
            cutsceneDialogue = dialogue;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;

            SceneManager.sceneLoaded += MapLoaded;
            if (dialogue != null) {
                onSceneLoad += StartDialogue;
            } else {
                onSceneLoad += UnlockMovement;
            }
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }

        public static void LoadCustsceneWithSetPosition(string map, Fog.Dialogue.Dialogue dialogue, Vector2 newPosition, Vector2 savePosition) {
            cutsceneDialogue = dialogue;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;

            OverrideCharacterPositions(savePosition);
            if (SaveLoader.AutoSave) {
                SaveLoader.SaveGame();
            }
            OverrideCharacterPositions(newPosition);

            SceneManager.sceneLoaded += MapLoaded;
            if (dialogue != null) {
                onSceneLoad += StartDialogue;
            } else {
                onSceneLoad += UnlockMovement;
            }
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }

        public static void LoadCustsceneFromBattle(string map, Fog.Dialogue.Dialogue dialogue) {
            cutsceneDialogue = dialogue;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;

            SceneManager.sceneLoaded += MapAndPositionsLoaded;
            if (dialogue != null) {
                onSceneLoad += StartDialogue;
            } else {
                onSceneLoad += UnlockMovement;
            }
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }
        #endregion

        public static void LoadMainMenu() {
            SceneManager.sceneLoaded += OnMainMenuLoad;
            beforeSceneChange?.Invoke();

            if (SaveLoader.AutoSave && SaveLoader.CanSaveGame) {
                Party.Instance.SaveOverworldPositions();
                SaveLoader.SaveGame();
            }

            SceneManager.LoadScene(mainMenuSceneName);
        }

        // Metodos que podem ser chamados ao carregar uma nova cena
        public static void OnBattleLoad(Scene map, LoadSceneMode mode) {
            BattleManager.instance.InitUnitsList(battleInfo.enemies);
            UpdateBackgroundImage();
            BattleManager.instance.PrepareBattle();
            PlayBattleBGM();

            battleInfo.Clear();
            SceneManager.sceneLoaded -= OnBattleLoad;
        }

        private static void UpdateBackgroundImage() {
            if (!battleInfo.BGImage)
                return;
            GameObject go = GameObject.Find(backgroundImageObjectName);
            if (go && go.TryGetComponent(out UnityEngine.UI.Image img))
                img.sprite = battleInfo.BGImage;
        }

        private static void PlayBattleBGM() {
            StaticReferences.BGM.Stop();
            if (battleInfo.BGM != null) {
                StaticReferences.BGM.PlaySong(battleInfo.BGM);
            } else {
                StaticReferences.BGM.Resume();
            }
        }

        public static void OnMainMenuLoad(Scene map, LoadSceneMode mode) {
            StaticReferences.BGM.PlaySong(StaticReferences.MainMenuBGM);
            SceneManager.sceneLoaded -= OnMainMenuLoad;
        }
        public static void MapAndPositionsLoaded(Scene map, LoadSceneMode mode) {
            RECalculator.encountersEnabled = false;
            Party.Instance.LoadOverworldPositions();
            RECalculator.encountersEnabled = true;
            SceneManager.sceneLoaded -= MapAndPositionsLoaded;
        }
        public static void MapLoaded(Scene map, LoadSceneMode mode) {
            RECalculator.encountersEnabled = true;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded -= MapLoaded;
        }
        public static void UnlockMovement() {
            CharacterOW.PartyCanMove = true;
            onSceneLoad -= UnlockMovement;
        }
        public static void StartDialogue() {
            Fog.Dialogue.Agent agent = CharacterOW.MainOWCharacter != null ? CharacterOW.MainOWCharacter.GetComponent<Fog.Dialogue.Agent>() : null;
            Fog.Dialogue.DialogueHandler.instance.StartDialogue(cutsceneDialogue);
            cutsceneDialogue = null;
            onSceneLoad -= StartDialogue;
        }
    }
}
