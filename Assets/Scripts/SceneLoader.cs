using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    public static class SceneLoader {
        static SceneLoader(){
            party = (Party)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets(" t:" + typeof(Party).ToString())[0]), typeof(Party));
        }
        // Essa referencia precisa ser configurada quando o jogo for inicializado
        public static Party party;
        public static Scene lastScene;
        private static List<Enemy> enemies;
        public static void LoadBattleScene(Enemy[] enemiesSelected, int[] enemyCount, Sprite BG, AudioClip BGM) {
            lastScene = SceneManager.GetActiveScene();
            // Adicionar o setup da batalha no SceneManager.sceneLoaded
            enemies = new List<Enemy>(enemiesSelected);
            SceneManager.sceneLoaded += OnBattleLoad;
            // Iniciar animação de encounter caso tenha (Ex.: tela escurecendo)
            // To do
            SceneManager.LoadScene("Battle");
        }
        public static void LoadOWScene(Scene map) {
            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.LoadScene(map.buildIndex);
        }
        public static void OnBattleLoad(Scene map, LoadSceneMode mode){
            // Adiciona os herois da party na lista de unidade da batalha
            foreach(Character character in party.characters){
                BattleManager.instance.units.Add(character.archetype);
            }
            // Adiciona os inimigos desejados para a lista de unidades da batalha
            BattleManager.instance.units.AddRange(enemies);
            // Avisa que a batalha pode começar
            BattleManager.instance.StartBattle();
            SceneManager.sceneLoaded -= OnBattleLoad;
        }
        public static void OnMapLoad(Scene map, LoadSceneMode mode) {
            // Desativa o calculo de encontrar batalhas para "teleportar" os personagens
            RECalculator.encountersEnabled = false;
            // Pegar a informação da posição dos personagens pelo SO da party
            // Reposicionar os game objects dos players na telas e recolocar referencias necessarias (Ex.: animator)
            // To do
            RECalculator.encountersEnabled = true;
            SceneManager.sceneLoaded -= OnMapLoad;
        }
    }
}
