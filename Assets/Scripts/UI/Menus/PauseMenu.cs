using UnityEngine;

namespace FinalInferno {
    public class PauseMenu : MonoBehaviour {
        public static PauseMenu Instance { get; private set; } = null;
        public static bool IsPaused { get; private set; } = false;

        [SerializeField] private UI.BestiaryMenu bestiaryMenu;

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
                return;
            }
            Instance = this;
            IsPaused = false;
        }

        public void ToggleBestiary() {
            if (bestiaryMenu)
                bestiaryMenu.ToggleBestiary();
        }

        public void ChangePauseState() {
            IsPaused = !IsPaused;
            CharacterOW.PartyCanMove = !IsPaused;
        }

        public void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }
    }
}