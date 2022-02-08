using UnityEngine;

namespace FinalInferno.UI {

    public class ChangeSceneUI : MonoBehaviour {
        private Animator Anim;

        [SerializeField] private LoadEnemiesPreview loadEnemiesPreview;

        // TO DO: Usar sistema de eventos ou delegate para a transição de cena ao inves desses public static
        public static bool isBattle = false;

        public static Sprite battleBG;
        public static AudioClip battleBGM;
        public static Enemy[] battleEnemies;

        public static string sceneName;
        public static Vector2 positionOnLoad;
        public static Vector2 savePosition;
        public static bool isCutscene;
        public static Fog.Dialogue.Dialogue selectedDialogue;

        private void Awake() {
            Anim = GetComponent<Animator>();
        }

        private void Update() {
            Anim.SetBool("IsBattle", isBattle);
        }

        private void ChangeMap() {
            if (!isCutscene) {
                SceneLoader.LoadOWScene(sceneName, true, positionOnLoad);
            } else {
                SceneLoader.LoadCustscene(sceneName, selectedDialogue, false, positionOnLoad, savePosition);
            }
        }

        public void SceneLoadCallback() {
            SceneLoader.onSceneLoad?.Invoke();
        }

        private void LoadPreview() {
            loadEnemiesPreview.LoadPreview();
        }

        private void StartBattle() {
            isBattle = false;
            SceneLoader.LoadBattleScene(battleEnemies, battleBG, battleBGM);
        }

        private void MainMenu() {
            SceneLoader.LoadMainMenu();
        }

        private void ReturnCheckpoint() {
            SaveLoader.LoadGame();
        }

        private void Continue() {
            if (isCutscene) {
                SceneLoader.LoadCustscene(Party.Instance.currentMap, selectedDialogue, true, null, null, true);
            } else {
                SceneLoader.LoadOWScene(Party.Instance.currentMap, true, null);
            }
        }
    }

}