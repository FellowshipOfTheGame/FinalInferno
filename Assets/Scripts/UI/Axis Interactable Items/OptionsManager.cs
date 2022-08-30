using UnityEngine;

namespace FinalInferno.UI.AII {
    public class OptionsManager : AIIManager {
        [Header("OptionItems")]
        [SerializeField] private ToggleItem autoSave;
        [SerializeField] private VolumeController volumeController;
        [SerializeField] private HorizontalSliderItem volumeMaster;
        [SerializeField] private HorizontalSliderItem bgmVolume;
        [SerializeField] private HorizontalSliderItem sfxVolume;
        [SerializeField] private HorizontalSliderItem sfxVolumeUI;

        public new void Awake() {
            currentItem = null;

            autoSave.Toggle(SaveLoader.AutoSave);
            autoSave.OnToggle += ToggleAutoSave;

            VolumeInfo volumeInfo = volumeController.GetInfo();

            volumeMaster.slider.value = volumeInfo.VolumeMaster;
            bgmVolume.slider.value = volumeInfo.VolumeBGM;
            sfxVolume.slider.value = volumeInfo.VolumeSFX;
            sfxVolumeUI.slider.value = volumeInfo.VolumeSFXUI;

            volumeMaster.slider.onValueChanged.AddListener(UpdateMaster);
            bgmVolume.slider.onValueChanged.AddListener(UpdateBGM);
            sfxVolume.slider.onValueChanged.AddListener(UpdateVFX);
            sfxVolumeUI.slider.onValueChanged.AddListener(UpdateVFXUI);
        }

        private void UpdateMaster(float value) {
            volumeController.SetMasterVolume(value);
        }

        private void UpdateBGM(float value) {
            volumeController.SetBGMVolume(value);
        }

        private void UpdateVFX(float value) {
            volumeController.SetSFXVolume(value);
        }

        private void UpdateVFXUI(float value) {
            volumeController.SetSFXUIVolume(value);
        }

        public new void Start() {
            active = false;
        }

        private void ToggleAutoSave() {
            if (AS) {
                AS.Play();
            }

            SaveLoader.AutoSave = !SaveLoader.AutoSave;
        }

        private void OnDisable() {
            PlayerPrefs.Save();
        }
    }
}