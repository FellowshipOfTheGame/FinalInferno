using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Info")]
    public class LoadInfoAction : ComponentRequester {
        private HeroInfo info;

        public override void Act(StateController controller) {
            info.LoadInfo(BattleManager.instance.CurrentUnit);
        }

        public override void RequestComponent(GameObject provider) {
            if (provider.TryGetComponent(out HeroInfo newInfo)) {
                info = newInfo;
            }
        }
    }
}