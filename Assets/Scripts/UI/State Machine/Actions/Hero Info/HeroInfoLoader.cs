using FinalInferno.UI.Battle.SkillMenu;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Hero Info Loader")]
    public class HeroInfoLoader : ComponentRequester {
        public HeroInfo Info { get; private set; }
        public override void RequestComponent(GameObject provider) {
            Info = provider.GetComponent<HeroInfo>();
        }
        public override void Act(StateController controller) { }
    }
}