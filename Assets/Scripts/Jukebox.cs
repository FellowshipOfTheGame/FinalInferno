using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Jukebox : MonoBehaviour {
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
        source.Stop();
        source.clip = null;
        source.playOnAwake = false;
    }

    public void Restart() {
        if (source.clip != null) {
            source.Stop();
        }

        source.Play();
    }

    public void Resume() {
        if (source.clip != null && !source.isPlaying) {
            source.Play();
        }
    }

    public void Pause() {
        if (source.clip != null && source.isPlaying) {
            source.Pause();
        }
    }

    public void Stop() {
        if (source.clip != null && source.isPlaying) {
            source.Stop();
        }
    }

    public void PlaySong(AudioClip clip, bool loop = true, bool forceRestart = false) {
        if (!(clip == source.clip && !forceRestart)) {
            source.Stop();
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }
    }
}
