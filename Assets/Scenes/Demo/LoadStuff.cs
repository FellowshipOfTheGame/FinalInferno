using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        public SceneAsset demoScene;
        public void Start(){
            //Party.Instance.currentMap = demoScene.name;
            AssetManager.LoadAllAssets();
            Quest exQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
            exQuest.events["TutorialComplete"] = false;
            Party.Instance.GiveExp(0);
            //SceneLoader.LoadOWScene(Party.Instance.currentMap);
            //SaveLoader.NewGame();
            SceneManager.LoadScene(demoScene.name);
        }
    }
}
