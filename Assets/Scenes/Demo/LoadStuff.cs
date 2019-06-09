using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        public void Start(){
            Debug.Log("StaticLoadStuff.gameStarted = " + StaticLoadStuff.gameStarted);
            if(!StaticLoadStuff.gameStarted){
                AssetManager.LoadAllAssets();
                Quest exQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
                exQuest.events["TutorialComplete"] = false;
                StaticLoadStuff.gameStarted = true;
            }
        }
    }
}
