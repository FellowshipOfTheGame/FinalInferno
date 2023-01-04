using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Current Unit Exists")]
    public class CurrentUnitExists : Decision {
        [SerializeField] private bool exists;

        public override bool Decide(StateController controller) {
            return exists == (BattleManager.instance.CurrentUnit != null);
        }
    }
}