using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FinalInferno.UI.AII{
    public class OptionsManager : AIIManager
    {
        [Header("OptionItems")]
        [SerializeField] private ToggleItem autoSave;
        [SerializeField] private AudioMixer audioMixer;
        // Supõe que o valor minimo dos sliders é 0
        // O valor maximo de cada slider define o volume máximo daquele canal
        [SerializeField] private HorizontalSliderItem volumeMaster;
        [SerializeField] private HorizontalSliderItem bgmVolume;
        [SerializeField] private HorizontalSliderItem sfxVolume;
        [SerializeField] private HorizontalSliderItem sfxVolumeUI;

        public new void Awake(){
            currentItem = null;

            autoSave.Toggle(SaveLoader.AutoSave);
            autoSave.OnToggle += ToggleAutoSave;

            float volume = 0f;

            audioMixer.GetFloat("VolumeMaster", out volume);
            volumeMaster.slider.value = Mathf.Pow(10, volume/20);
            volumeMaster.slider.onValueChanged.AddListener(UpdateMaster);

            audioMixer.GetFloat("VolumeBGM", out volume);
            bgmVolume.slider.value = Mathf.Pow(10, volume/20);
            bgmVolume.slider.onValueChanged.AddListener(UpdateBGM);
            
            audioMixer.GetFloat("VolumeSFX", out volume);
            sfxVolume.slider.value = Mathf.Pow(10, volume/20);
            sfxVolume.slider.onValueChanged.AddListener(UpdateVFX);

            audioMixer.GetFloat("VolumeSFXUI", out volume);
            sfxVolumeUI.slider.value = Mathf.Pow(10, volume/20);
            sfxVolumeUI.slider.onValueChanged.AddListener(UpdateVFXUI);
        }

        void UpdateMaster(float value){
            float newVolume = Mathf.Log10(Mathf.Max((value), 0.0001f)) * 20f;
            UpdateVolumeChannel("VolumeMaster", newVolume);
        }

        void UpdateBGM(float value){
            float newVolume = Mathf.Log10(Mathf.Max((value), 0.0001f)) * 20f;
            UpdateVolumeChannel("VolumeBGM", newVolume);
        }

        void UpdateVFX(float value){
            float newVolume = Mathf.Log10(Mathf.Max((value), 0.0001f)) * 20f;
            UpdateVolumeChannel("VolumeSFX", newVolume);
        }

        void UpdateVFXUI(float value){
            float newVolume = Mathf.Log10(Mathf.Max((value), 0.0001f)) * 20f;
            UpdateVolumeChannel("VolumeSFXUI", newVolume);
        }

        void UpdateVolumeChannel(string channel, float newVolume){
            if (AS) AS.Play();
            audioMixer.SetFloat(channel, newVolume);
            PlayerPrefs.SetFloat(channel, newVolume);
        }

        public new void Start(){
            active = false;
        }

        void ToggleAutoSave(){
            if (AS) AS.Play();
            SaveLoader.AutoSave = !SaveLoader.AutoSave;
            PlayerPrefs.SetString("autosave", (SaveLoader.AutoSave? "true" : "false"));
        }

        void OnDisable(){
            PlayerPrefs.Save();
        }
    }
}