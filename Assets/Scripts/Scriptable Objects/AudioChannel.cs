using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioChannel {
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
        float value;
        if (!mixer.GetFloat(VolumeString, out value)) {
            Debug.LogError($"Variable {VolumeString} not set in audio mixer");
            return 1f;
        }
        value = Mathf.Pow(10, value / 20f);
        value = Mathf.Clamp(value, minVolume, maxVolume);
        return (value - minVolume) / (maxVolume - minVolume);
    }
}
