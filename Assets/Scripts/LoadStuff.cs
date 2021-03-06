﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        [SerializeField] private bool demo = false;
        [SerializeField] private VolumeController volumeController = null;
        private bool assetsLoaded = false;
        // Start is called before the first frame update
        void Start(){
            Cursor.visible = false;
            assetsLoaded = false;
            volumeController.ResetValues();
            SaveLoader.AutoSave = true;
            
            StartCoroutine(PreloadAsync());
        }

        private IEnumerator PreloadAsync(){
            yield return new WaitForEndOfFrame();

            #if UNITY_EDITOR
            // No editor recarrega as tabelas por precaução
            StaticReferences.AssetManager.BuildDatabase();
            #endif

            AssetManager.Preload();
            yield return new WaitForSeconds(1.5f);
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