using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;
using FinalInferno.UI.AII;

namespace FinalInferno.UI.FSM
{
    /// <summary>
    /// Ação que toca uma musica na jukebox de BGM.
    /// </summary>
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Play Sound Effect")]
    public class PlaySFX : ComponentRequester
    {
    public AudioClip clip;
    private AudioSource source;

        public override void Act(StateController controller)
        {
            source.clip = clip;
            source.Play();
        }

        public override void RequestComponent(GameObject provider)
        {
            source = provider.GetComponent<AudioSource>();
        }

    }

}