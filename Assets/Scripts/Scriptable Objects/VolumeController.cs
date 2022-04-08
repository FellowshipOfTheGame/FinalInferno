using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "VolumeController", menuName = "ScriptableObject/VolumeController")]
public class VolumeController : ScriptableObject {
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