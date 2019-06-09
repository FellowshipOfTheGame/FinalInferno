using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    public static class SceneLoader {
        // Essa referencia precisa ser configurada quando o jogo for inicializado
        private static int lastOWSceneID;
        public static int LastOWSceneID{ get{ return lastOWSceneID; } }
        private static List<Enemy> enemies;
        private static bool updatePositions;

        public static void LoadBattleScene(Enemy[] enemiesSelected, Sprite BG, AudioClip BGM) {
            RECalculator.encountersEnabled = false;
            lastOWSceneID = SceneManager.GetActiveScene().buildIndex;
            // Armazena a posicao dos personagens no overworld dentro do SO correspondente
            for(int i = 0; i < Party.Instance.characters.Count; i++){
                Party.Instance.characters[i].position = CharacterOW.CharacterList[i].transform.position;
            }
            // Adicionar o setup da batalha no SceneManager.sceneLoaded
            enemies = new List<Enemy>(enemiesSelected);
            SceneManager.sceneLoaded += OnBattleLoad;
            // To do: Iniciar animação de encounter caso tenha (Ex.: tela escurecendo)
            SceneManager.LoadScene("Battle");
        }

        public static void LoadOWScene(Scene map, bool shouldUpdate = false) {
            updatePositions = shouldUpdate;
            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.LoadScene(map.buildIndex);
        }
        public static void LoadOWScene(string map, bool shouldUpdate = false) {
            updatePositions = shouldUpdate;
            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.LoadScene(map);
        }
        public static void LoadOWScene(int mapID, bool shouldUpdate = false) {
            updatePositions = shouldUpdate;
            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.LoadScene(mapID);
        }

        public static void LoadMainMenu(){
            // Simplesmente carregar a cena não deveria dar nenhum problema
            // Mas a função intermediaria ficara aqui para o caso de querermos fazer algo diferente
            SceneManager.LoadScene("MainMenu");
        }

        public static void OnBattleLoad(Scene map, LoadSceneMode mode){
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
                    CharacterOW.CharacterList[i].transform.position = Party.Instance.characters[i].position;
                }
            }
            RECalculator.encountersEnabled = true;
            lastOWSceneID = SceneManager.GetActiveScene().buildIndex;
            SceneManager.sceneLoaded -= OnMapLoad;
        }
    }
}
