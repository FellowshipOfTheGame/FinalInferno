using UnityEngine;

namespace FinalInferno.UI {

    public class ChangeSceneUI : MonoBehaviour {
        private const string IsBattleAnimString = "IsBattle";
        private Animator anim;
        private bool hasIsBattleParameter;
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
            anim = GetComponent<Animator>();
            hasIsBattleParameter = System.Array.Find(anim.parameters, parameter => parameter.name == IsBattleAnimString) != null;
        }

        private void Update() {
            if (hasIsBattleParameter)
                anim.SetBool(IsBattleAnimString, isBattle);
        }

        private void MainMenu() {
            SceneLoader.LoadMainMenu();
        }

        // Overworld callbacks
        private void ChangeMap() {
            if (!isCutscene) {
                SceneLoader.LoadOWSceneWithSetPosition(sceneName, positionOnLoad);
            } else {
                SceneLoader.LoadCustsceneWithSetPosition(sceneName, selectedDialogue, positionOnLoad, savePosition);
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
            SceneLoader.LoadBattleScene(new BattleInfo(battleEnemies, battleBG, battleBGM));
        }

        // Battle callbacks
        private void ReturnCheckpoint() {
            SaveLoader.LoadGame();
        }

        private void Continue() {
            if (isCutscene) {
                SceneLoader.LoadCustsceneFromBattle(Party.Instance.currentMap, selectedDialogue);
            } else {
                SceneLoader.LoadOWSceneFromBattle(Party.Instance.currentMap);
            }
        }
    }

}