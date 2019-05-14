using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    public static class SceneLoader {
        // Essa referencia precisa ser configurada quando o jogo for inicializado
        public static Party party;
        public static Scene lastScene;
        public static void LoadBattleScene(Enemy[] enemies, int[] enemyCount, Sprite BG, AudioClip BGM) {
            lastScene = SceneManager.GetActiveScene();
            // Iniciar animação de encounter caso tenha (Ex.: tela escurecendo)
            // Adicionar o setup da batalha no SceneManager.sceneLoaded
            // To do
            SceneManager.LoadScene("Battle");
        }
        public static void LoadOWScene(Scene map) {
            SceneManager.sceneLoaded += OnMapLoad;
            SceneManager.LoadScene(map.buildIndex);
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
