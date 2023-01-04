using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Save Game")]
    public class CanSaveGame : Decision {
        [SerializeField] private bool canSave;

        public override bool Decide(StateController controller) {
            return canSave == SaveLoader.CanSaveGame;
        }
    }
}