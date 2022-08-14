using TMPro;
using UnityEngine;

namespace FinalInferno.UI {
    public class MenuScript : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI versionText;

        private void Awake() {
            if (versionText)
                versionText.text = $"Version {Application.version}";
        }

        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            SceneLoader.LoadMainMenu();
#else
            Application.Quit();
#endif
        }

        public void SceneLoadCallback() {
            SceneLoader.onSceneLoad?.Invoke();
        }

        public void NewGame() {
            SaveLoader.NewGame();
        }

        public void LoadGame() {
            SaveLoader.LoadGame();
        }
    }
}