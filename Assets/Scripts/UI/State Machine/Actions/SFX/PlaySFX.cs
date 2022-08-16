using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Play Sound Effect")]
    public class PlaySFX : ComponentRequester {
        public AudioClip clip;
        private AudioSource source;

        public override void Act(StateController controller) {
            source.clip = clip;
            source.Play();
        }

        public override void RequestComponent(GameObject provider) {
            source = provider.GetComponent<AudioSource>();
        }
    }
}