using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/End Battle")]
    public class EndBattleDecision : Decision {
        [SerializeField] private VictoryType desiredType;

        public override bool Decide(StateController controller) {
            return BattleManager.instance.CheckEnd() == desiredType;
        }
    }
}
