using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    public class TriggerSceneChange : Triggerable
    {
        [SerializeField] private UnityEditor.SceneAsset newScene;
        protected override void TriggerAction(Fog.Dialogue.Agent agent){
            if(newScene != null){
                SceneLoader.LoadOWScene(newScene.name);
            }
        }
    }
}
