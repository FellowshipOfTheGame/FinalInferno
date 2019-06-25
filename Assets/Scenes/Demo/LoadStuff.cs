﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        public void Start(){
            AssetManager.LoadAllAssets();
            Party.Instance.currentMap = "Demo";
            Quest exQuest = AssetManager.LoadAsset<Quest>("AdventurerQuest");
            exQuest.events["TutorialComplete"] = false;
            Party.Instance.GiveExp(0);
            SceneLoader.LoadOWScene(Party.Instance.currentMap);
        }
    }
}
