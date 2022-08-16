using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Scene Change Callback")]
    public class SceneChangeCallback : Action {
        public override void Act(StateController controller) {
            SceneLoader.onSceneLoad?.Invoke();
        }
    }
}