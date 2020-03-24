using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace FinalInferno{
    public static class SceneLoader {
        private static List<Enemy> enemies = new List<Enemy>();
        private static bool updatePositions = false;
        private static Fog.Dialogue.Dialogue cutsceneDialogue = null;
        private static AudioClip battleBGM;
        public static UnityAction beforeSceneChange = null;
        public static UnityAction onSceneLoad = null;

        public static void LoadBattleScene(Enemy[] enemiesSelected, Sprite BG, AudioClip BGM) {
            RECalculator.encountersEnabled = false;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            // Armazena a posicao dos personagens no overworld dentro do SO correspondente
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                if(CharacterOW.CharacterList[i] != null)
                    Party.Instance.characters[i].position = CharacterOW.CharacterList[i].transform.position;
            }
            // Adicionar o setup da batalha no SceneManager.sceneLoaded
            enemies = new List<Enemy>(enemiesSelected);
            // Salva a BGM de batalha
            battleBGM = BGM;
            // O callback de batalha deve usar o callback do scene manager padrão,
            // porque a primeira transição da maquina de estado espera que isso seja chamado
            SceneManager.sceneLoaded += OnBattleLoad;

            beforeSceneChange?.Invoke();
            SceneManager.LoadScene("Battle");
        }


        // Métodos que carregam a cena desejada
        public static void LoadOWScene(Scene map, bool shouldUpdate = false, Vector2? newPosition = null, bool dontSave = false) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = map.name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave && !dontSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map.buildIndex);
        }
        public static void LoadOWScene(string map, bool shouldUpdate = false, Vector2? newPosition = null, bool dontSave = false) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = map;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave && !dontSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(map);
        }
        public static void LoadOWScene(int mapID, bool shouldUpdate = false, Vector2? newPosition = null, bool dontSave = false) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = SceneManager.GetSceneByBuildIndex(mapID).name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave && !dontSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            onSceneLoad += UnlockMovement;
            beforeSceneChange?.Invoke();
            SceneManager.LoadScene(mapID);
        }

        // Métodos que carregam a cena e iniciam um diálogo
        public static void LoadCustscene(Scene map, Fog.Dialogue.Dialogue dialogue, Vector2? newPosition = null) {
            updatePositions = false;
            cutsceneDialogue = dialogue;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave)
                SaveLoader.SaveGame();

            if(dialogue != null){
                SceneManager.sceneLoaded += OnMapLoad;
                onSceneLoad += StartDialogue;
                beforeSceneChange?.Invoke();
                SceneManager.LoadScene(map.buildIndex);
            }else{
                SceneManager.sceneLoaded += OnMapLoad;
                onSceneLoad += UnlockMovement;
                beforeSceneChange?.Invoke();
                SceneManager.LoadScene(map.buildIndex);
            }
        }
        public static void LoadCustscene(string map, Fog.Dialogue.Dialogue dialogue, Vector2? newPosition = null) {
            updatePositions = false;
            cutsceneDialogue = dialogue;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave)
                SaveLoader.SaveGame();

            if(dialogue != null){
                SceneManager.sceneLoaded += OnMapLoad;
                onSceneLoad += StartDialogue;
                beforeSceneChange?.Invoke();
                SceneManager.LoadScene(map);
            }else{
                SceneManager.sceneLoaded += OnMapLoad;
                onSceneLoad += UnlockMovement;
                beforeSceneChange?.Invoke();
                SceneManager.LoadScene(map);
            }
        }

        public static void LoadMainMenu(){
            SceneManager.sceneLoaded += OnMainMenuLoad;
            beforeSceneChange?.Invoke();

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave && SaveLoader.CanSaveGame){
                for(int i = 0; i < Party.Capacity; i++){
                    Party.Instance.characters[i].position = new Vector2(CharacterOW.CharacterList[i].transform.position.x, CharacterOW.CharacterList[i].transform.position.y);
                }
                SaveLoader.SaveGame();
            }
                
            SceneManager.LoadScene("MainMenu");
        }

        // Metodos que podem ser chamados ao carregar uma nova cena
        public static void OnBattleLoad(Scene map, LoadSceneMode mode){
            BattleManager.instance.units.Clear();
            // Adiciona os herois da party na lista de unidade da batalha
            foreach(Character character in Party.Instance.characters){
                BattleManager.instance.units.Add(character.archetype);
            }
            // Adiciona os inimigos desejados para a lista de unidades da batalha
            BattleManager.instance.units.AddRange(enemies);
            // Avisa que a batalha pode começar
            BattleManager.instance.PrepareBattle();
            // Se não houver música, deixa a música padrão que está configurada
            StaticReferences.instance.BGM.Stop();
            if(battleBGM != null)
                StaticReferences.instance.BGM.PlaySong(battleBGM);
            else
                StaticReferences.instance.BGM.Resume();

            battleBGM = null;
            SceneManager.sceneLoaded -= OnBattleLoad;
        }
        public static void OnMainMenuLoad(Scene map, LoadSceneMode mode){
            StaticReferences.instance.BGM.PlaySong(StaticReferences.instance.mainMenuBGM);
            SceneManager.sceneLoaded -= OnMainMenuLoad;
        }
        public static void OnMapLoad(Scene map, LoadSceneMode mode) {
            if(updatePositions){
                // Desativa o calculo de encontrar batalhas para "teleportar" os personagens
                RECalculator.encountersEnabled = false;
                // Pegar a informação da posição dos personagens pelo SO da party
                // Reposicionar os game objects dos players na tela
                for(int i = 0; i < Party.Instance.characters.Count; i++){
                    if(CharacterOW.CharacterList[i] != null)
                        CharacterOW.CharacterList[i].transform.position = Party.Instance.characters[i].position;
                }
            }
            RECalculator.encountersEnabled = true;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded -= OnMapLoad;
        }
        public static void UnlockMovement(){
            CharacterOW.PartyCanMove = true;
            onSceneLoad -= UnlockMovement;
        }
        public static void StartDialogue(){
            Fog.Dialogue.Agent agent = CharacterOW.MainOWCharacter?.GetComponent<Fog.Dialogue.Agent>();
            Fog.Dialogue.DialogueHandler.instance.StartDialogue(cutsceneDialogue);
            cutsceneDialogue = null;
            onSceneLoad -= StartDialogue;
        }
    }
}
