using UnityEngine;

[RequireComponent(typeof(Jukebox), typeof(DontDestroyThis))]
public class StaticReferences : MonoBehaviour {
    public static StaticReferences Instance { get; private set; } = null;
    public static bool DebugBuild {
        get {
#if UNITY_EDITOR
            return true;
#endif
            if (Instance) {
                return Instance.debugBuild;
            }
            return false;
        }
    }
    public static Jukebox BGM {
        get {
            if (Instance) {
                return Instance.bgm;
            }
            return null;
        }
    }
    [SerializeField] private bool debugBuild;
    [SerializeField] private Jukebox bgm;
    [SerializeField] private AudioClip mainMenuBGM;
    public static AudioClip MainMenuBGM => Instance?.mainMenuBGM;
    [SerializeField] private Fog.Dialogue.Dialogue firstDialogue;
    public static Fog.Dialogue.Dialogue FirstDialogue => Instance?.firstDialogue;
    [SerializeField] private FinalInferno.ScenePicker firstScene;
    public static string FirstScene => (Instance != null && Instance.firstScene.Name != "") ? Instance.firstScene.Name : null;
    [SerializeField] private FinalInferno.AssetManager assetManager = null;
    public static FinalInferno.AssetManager AssetManager => Instance?.assetManager;
    [SerializeField] private VolumeController volumeController;
    public static VolumeController VolumeController => Instance?.volumeController;

    // Start is called before the first frame update
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        bgm = GetComponent<Jukebox>();
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }
}