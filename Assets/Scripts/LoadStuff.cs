using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake(){
            AssetManager.LoadAllAssets();
        }
    }
}