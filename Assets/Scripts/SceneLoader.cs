using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    public static class SceneLoader {
        // Essa referencia precisa ser configurada quando o jogo for inicializado
        //private static int lastOWSceneID = 0;
        //public static int LastOWSceneID{ get{ return lastOWSceneID; } }
        private static List<Enemy> enemies = new List<Enemy>();
        private static bool updatePositions = false;
        private static Fog.Dialogue.Dialogue cutsceneDialogue = null;

        public static void LoadBattleScene(Enemy[] enemiesSelected, Sprite BG, AudioClip BGM) {
            RECalculator.encountersEnabled = false;
            // Essa atualização do mapa so e necessaria para a demo
            //lastOWSceneID = SceneManager.GetActiveScene().buildIndex;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            // Armazena a posicao dos personagens no overworld dentro do SO correspondente
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                if(CharacterOW.CharacterList[i] != null)
                    Party.Instance.characters[i].position = CharacterOW.CharacterList[i].transform.position;
            }
            // Adicionar o setup da batalha no SceneManager.sceneLoaded
            enemies = new List<Enemy>(enemiesSelected);
            SceneManager.sceneLoaded += OnBattleLoad;
            // To do: Iniciar animação de encounter caso tenha (Ex.: tela escurecendo)
            // A animação também pode ser iniciada pelo RECalculator, ante de chamar este metodo
            SceneManager.LoadScene("Battle");
        }


        // Métodos que carregam a cena desejada
        public static void LoadOWScene(Scene map, bool shouldUpdate = false, Vector2? newPosition = null) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = map.name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.sceneLoaded += UnlockMovement;
            SceneManager.LoadScene(map.buildIndex);
        }
        public static void LoadOWScene(string map, bool shouldUpdate = false, Vector2? newPosition = null) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = map;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.sceneLoaded += UnlockMovement;
            SceneManager.LoadScene(map);
        }
        public static void LoadOWScene(int mapID, bool shouldUpdate = false, Vector2? newPosition = null) {
            updatePositions = shouldUpdate;
            if(newPosition != null){
                foreach(Character character in Party.Instance.characters){
                    character.position = newPosition.Value;
                }
            }
            Party.Instance.currentMap = SceneManager.GetSceneByBuildIndex(mapID).name;

            // Salva o jogo se o autosave esta ativado
            if(SaveLoader.AutoSave)
                SaveLoader.SaveGame();

            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.sceneLoaded += UnlockMovement;
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
                SceneManager.sceneLoaded += StartDialogue;
                SceneManager.LoadScene(map.buildIndex);
            }else{
                SceneManager.sceneLoaded += OnMapLoad;
                SceneManager.sceneLoaded += UnlockMovement;
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
                SceneManager.sceneLoaded += StartDialogue;
                SceneManager.LoadScene(map);
            }else{
                SceneManager.sceneLoaded += OnMapLoad;
                SceneManager.sceneLoaded += UnlockMovement;
                SceneManager.LoadScene(map);
            }
        }

        public static void LoadMainMenu(){
            // Simplesmente carregar a cena não deveria dar nenhum problema
            // Mas a função intermediaria ficara aqui para o caso de querermos fazer algo diferente
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
            BattleManager.instance.StartBattle();
            SceneManager.sceneLoaded -= OnBattleLoad;
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
            //lastOWSceneID = SceneManager.GetActiveScene().buildIndex;
            Party.Instance.currentMap = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded -= OnMapLoad;
        }
        public static void UnlockMovement(Scene map, LoadSceneMode mode){
            CharacterOW.PartyCanMove = true;
            SceneManager.sceneLoaded -= UnlockMovement;
        }
        public static void StartDialogue(Scene map, LoadSceneMode mode){
            Fog.Dialogue.Agent agent = CharacterOW.MainOWCharacter.GetComponent<Fog.Dialogue.Agent>();
            Fog.Dialogue.DialogueHandler.instance.StartDialogue(cutsceneDialogue, agent, agent.GetComponent<Movable>());
            cutsceneDialogue = null;
            SceneManager.sceneLoaded -= StartDialogue;
        }
    }
}
