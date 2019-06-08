using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        public static bool gameStarted = false;

        public void Start(){
            if(!gameStarted){
                AssetManager.LoadAllAssets();
                Quest exQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
                exQuest.events["TutorialComplete"] = false;
                gameStarted = true;
                Debug.Log("suahsuasuahus");
            }
        }
    }
}
