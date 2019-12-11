using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start(){
            Cursor.visible = false;
            AssetManager.LoadAllAssets();
            SceneLoader.LoadMainMenu();
        }
    }
}