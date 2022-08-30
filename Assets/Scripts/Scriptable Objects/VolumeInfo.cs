using UnityEngine;

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
