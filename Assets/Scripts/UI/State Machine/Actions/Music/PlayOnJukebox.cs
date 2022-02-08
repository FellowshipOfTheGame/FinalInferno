using UnityEngine;

namespace FinalInferno.UI.FSM {
    /// <summary>
    /// Ação que toca uma musica na jukebox de BGM.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Play On Jukebox")]
    public class PlayOnJukebox : Action {
        public AudioClip clip;
        public bool loop;
        /// <summary>
        /// Executa uma ação.
        /// </summary>
        /// <param name="controller"> O controlador da máquina de estados. </param>

        public override void Act(StateController controller) {
            StaticReferences.BGM.PlaySong(clip, loop);
        }

    }

}