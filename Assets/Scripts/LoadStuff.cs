using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        [SerializeField] private bool demo = false;
        [SerializeField] private AudioMixer mixer = null;
        // Start is called before the first frame update
        void Start(){
            Cursor.visible = false;
            AssetManager.LoadAllAssets();
            string[] channels = new string[] {"VolumeMaster", "VolumeBGM", "VolumeSFX", "VolumeSFXUI"};
            foreach(string channel in channels){
                mixer.SetFloat(channel, PlayerPrefs.GetFloat(channel));
            }
            SaveLoader.AutoSave = PlayerPrefs.GetString("autosave") == "true";
            
            if(demo){
                SaveLoader.StartDemo();
            }else{
                SceneLoader.LoadMainMenu();
            }
        }
    }
}