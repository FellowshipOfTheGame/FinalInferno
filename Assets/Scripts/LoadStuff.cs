using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        [SerializeField] private bool demo = false;
        [SerializeField] private AudioMixer mixer = null;
        private bool assetsLoaded = false;
        // Start is called before the first frame update
        void Start(){
            Cursor.visible = false;
            assetsLoaded = false;
            // TO DO add/load playerprefs to/from save files
            string[] channels = new string[] {"VolumeMaster", "VolumeBGM", "VolumeSFX", "VolumeSFXUI"};
            foreach(string channel in channels){
                if(PlayerPrefs.HasKey(channel)){
                    mixer.SetFloat(channel, PlayerPrefs.GetFloat(channel));
                }
            }
            if(PlayerPrefs.HasKey("autosave")){
                SaveLoader.AutoSave = PlayerPrefs.GetString("autosave") == "true";
            }else{
                SaveLoader.AutoSave = true;
            }
            
            StartCoroutine(PreloadAsync());
        }

        private IEnumerator PreloadAsync(){
            yield return new WaitForEndOfFrame();

            #if UNITY_EDITOR
            // No editor recarrega as tabelas por precaução
            StaticReferences.AssetManager.BuildDatabase();
            #endif

            AssetManager.Preload();
            yield return new WaitForSeconds(2f);
            assetsLoaded = true;
        }

        void Update(){
            if(assetsLoaded){
                if(demo){
                    SaveLoader.StartDemo();
                }else{
                    SceneLoader.LoadMainMenu();
                }
            }
        }
    }
}