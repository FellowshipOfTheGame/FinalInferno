using UnityEngine;

namespace FinalInferno.UI.FSM {
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Play On Jukebox")]
    public class PlayOnJukebox : Action {
        public AudioClip clip;
        public bool loop;

        public override void Act(StateController controller) {
            StaticReferences.BGM.PlaySong(clip, loop);
        }
    }
}