using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Save Game")]
    public class SaveGame : Action {
        public override void Act(StateController controller) {
            Party.Instance.SaveOverworldPositions();
            SaveLoader.SaveGame();
        }
    }
}