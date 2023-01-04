using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Can Party Move")]
    public class CanCharactersMove : Decision {
        [SerializeField] private bool canMove;

        public override bool Decide(StateController controller) {
            return canMove == CharacterOW.PartyCanMove;
        }
    }
}
