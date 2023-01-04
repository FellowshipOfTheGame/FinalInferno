using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Bestiary State")]
    public class ChangeBestiaryState : ComponentRequester {
        private BestiaryMenu bestiary;
        [SerializeField] private bool shouldOpen;

        public override void Act(StateController controller) {
            if (shouldOpen)
                bestiary.OpenBestiary();
            else
                bestiary.CloseBestiary();
        }

        public override void RequestComponent(GameObject provider) {
            bestiary = provider.GetComponent<BestiaryMenu>();
        }
    }
}