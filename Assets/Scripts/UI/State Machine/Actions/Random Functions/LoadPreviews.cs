using FinalInferno.UI.Saves;
using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/LoadPreviews")]
    public class LoadPreviews : Action {
        public SlotsList list = null;

        public override void Act(StateController controller) {
            list.UpdateSlotsContent(SaveLoader.PreviewAllSlots());
        }
    }
}