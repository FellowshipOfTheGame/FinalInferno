using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jukebox), typeof(DontDestroyThis))]
public class StaticReferences : MonoBehaviour
{
    public static StaticReferences Instance { get; private set; } = null;
    public static bool DebugBuild{
        get{
        #if UNITY_EDITOR
            return true;
        #endif
            if(Instance){
                return Instance.debugBuild;
            }
            return false;
        }
    }
    public static Jukebox BGM{
        get{
            if(Instance){
                return Instance.bgm;
            }
            return null;
        }
    }
    [SerializeField] private bool debugBuild;
    [SerializeField] private Jukebox bgm;
    [SerializeField] private AudioClip mainMenuBGM;
    public static AudioClip MainMenuBGM { get => (Instance == null)? null : Instance.mainMenuBGM; }
    [SerializeField] private Fog.Dialogue.Dialogue firstDialogue;
    public static Fog.Dialogue.Dialogue FirstDialogue { get => (Instance == null)? null:  Instance.firstDialogue; }
    [SerializeField] private FinalInferno.ScenePicker firstScene;
    public static string FirstScene { get => (Instance != null && Instance.firstScene.Name != "")? Instance.firstScene.Name : null; }
    [SerializeField] private FinalInferno.AssetManager assetManager = null;
    public static FinalInferno.AssetManager AssetManager { get => (Instance == null)? null : Instance.assetManager; }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(this);
        }

        bgm = GetComponent<Jukebox>();
    }

    void OnDestroy(){
        if(Instance == this){
            Instance = null;
        }
    }
}