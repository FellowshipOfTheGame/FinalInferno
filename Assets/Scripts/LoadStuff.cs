using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class LoadStuff : MonoBehaviour
    {
        [SerializeField] private bool demo = false;
        // Start is called before the first frame update
        void Start(){
            Cursor.visible = false;
            AssetManager.LoadAllAssets();
            if(demo){
                SaveLoader.StartDemo();
            }else{
                SceneLoader.LoadMainMenu();
            }
        }
    }
}