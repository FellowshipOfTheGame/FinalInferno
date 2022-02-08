using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Change Party Movement")]
    public class ChangePartyMovement : Action {
        [SerializeField] private bool move;

        public override void Act(StateController controller) {
            CharacterOW.PartyCanMove = move;
        }
    }
}