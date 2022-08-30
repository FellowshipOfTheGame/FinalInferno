using FinalInferno;
using Fog.Dialogue;
using UnityEngine;

[RequireComponent(typeof(Jukebox), typeof(DontDestroyThis))]
public class StaticReferences : MonoBehaviour {
    public static StaticReferences Instance { get; private set; } = null;
    [SerializeField] private bool debugBuild;
    public static bool DebugBuild {
        get {
#if UNITY_EDITOR
            return true;
#endif
            return Instance ? Instance.debugBuild : false;
        }
    }
    [SerializeField] private Jukebox bgm;
    public static Jukebox BGM => Instance ? Instance.bgm : null;
    [SerializeField] private AudioClip mainMenuBGM;
    public static AudioClip MainMenuBGM => Instance == null ? null : Instance.mainMenuBGM;
    [SerializeField] private Dialogue firstDialogue;
    public static Dialogue FirstDialogue => Instance == null ? null : Instance.firstDialogue;
    [SerializeField] private ScenePicker firstScene;
    public static string FirstScene => (Instance != null && Instance.firstScene.Name != "") ? Instance.firstScene.Name : null;
    [SerializeField] private AssetManager assetManager = null;
    public static AssetManager AssetManager => Instance == null ? null : Instance.assetManager;
    [SerializeField] private VolumeController volumeController;
    public static VolumeController VolumeController => Instance == null ? null : Instance.volumeController;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        if (!bgm)
            bgm = GetComponent<Jukebox>();
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }
}