using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        #if UNITY_EDITOR
        public SceneAsset demoScene;
        #endif
        public void Start(){
            AssetManager.LoadAllAssets();
        #if UNITY_EDITOR
            Quest exQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
            exQuest.events["TutorialComplete"] = false;
            Party.Instance.GiveExp(0);
            //Party.Instance.currentMap = demoScene.name;
            //SceneLoader.LoadOWScene(Party.Instance.currentMap);
            //SaveLoader.NewGame();
            SceneManager.LoadScene(demoScene.name);
        #else
            SceneManager.LoadScene("MainMenu");
        #endif
        }
    }
}
