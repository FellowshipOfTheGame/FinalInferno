using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "VolumeController", menuName = "ScriptableObject/VolumeController")]
public class VolumeController : ScriptableObject {
    #region subclasses
    [System.Serializable]
    private class AudioChannel {
        // All Volume strings must be "Volume" + channelName
        [SerializeField] private string volumeString;
        public string VolumeString => volumeString;
        // [SerializeField,HideInInspector] private float minVolume = 0.0001f;
        private const float minVolume = 0.0001f;
        public float MinVolume => minVolume;
        [SerializeField, Range(0.1f, 1)] private float maxVolume = 1;
        public float MaxVolume => maxVolume;

        public void SetValue(AudioMixer mixer, float value) {
            value = Mathf.Clamp(value, 0, 1f);
            PlayerPrefs.SetFloat(VolumeString, value);
            value = MinVolume + value * (MaxVolume - MinVolume);
            if (!mixer.SetFloat(VolumeString, Mathf.Log10(value) * 20f)) {
                Debug.LogError($"Variable {VolumeString} not set in audio mixer");
            }
        }

        public float CurrentValue(AudioMixer mixer) {
            float value = 1f;
            if (mixer.GetFloat(VolumeString, out value)) {
                value = Mathf.Pow(10, value / 20f);
                value = Mathf.Clamp(value, minVolume, maxVolume);
                return (value - minVolume) / (maxVolume - minVolume);
            } else {
                Debug.LogError($"Variable {VolumeString} not set in audio mixer");
                return 1f;
            }
        }
    }

    [System.Serializable]
    public class VolumeInfo {
        [SerializeField] private float volumeMaster = 1f;
        public float VolumeMaster => volumeMaster;
        [SerializeField] private float volumeBGM = 1f;
        public float VolumeBGM => volumeBGM;
        [SerializeField] private float volumeSFX = 1f;
        public float VolumeSFX => volumeSFX;
        [SerializeField] private float volumeSFXUI = 1f;
        public float VolumeSFXUI => volumeSFXUI;

        public VolumeInfo() : this(1f, 1f, 1f, 1f) { }
        public VolumeInfo(float master, float bgm, float sfx, float sfxui) {
            volumeMaster = master;
            volumeBGM = bgm;
            volumeSFX = sfx;
            volumeSFXUI = sfxui;
        }
    }
    #endregion

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioChannel masterChannel;
    [SerializeField] private AudioChannel BGMChannel;
    [SerializeField] private AudioChannel SFXChannel;
    [SerializeField] private AudioChannel SFXUIChannel;

    public VolumeInfo GetInfo() {
        float master = masterChannel.CurrentValue(audioMixer);
        float bgm = BGMChannel.CurrentValue(audioMixer);
        float sfx = SFXChannel.CurrentValue(audioMixer);
        float sfxui = SFXUIChannel.CurrentValue(audioMixer);
        return new VolumeInfo(master, bgm, sfx, sfxui);
    }

    public void ResetValues(VolumeInfo info = null) {
        info ??= new VolumeInfo();
        masterChannel.SetValue(audioMixer, info.VolumeMaster);
        BGMChannel.SetValue(audioMixer, info.VolumeBGM);
        SFXChannel.SetValue(audioMixer, info.VolumeSFX);
        SFXUIChannel.SetValue(audioMixer, info.VolumeSFXUI);
    }

    private void SetVolume(AudioChannel channel, float value) {
        channel.SetValue(audioMixer, value);
    }
    public void SetMasterVolume(float value) {
        SetVolume(masterChannel, value);
    }
    public void SetBGMVolume(float value) {
        SetVolume(BGMChannel, value);
    }
    public void SetSFXVolume(float value) {
        SetVolume(SFXChannel, value);
    }
    public void SetSFXUIVolume(float value) {
        SetVolume(SFXUIChannel, value);
    }
}