using UnityEditor;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "AssetDatabase", menuName = "ScriptableObject/Database")]
    public class AssetManager : ScriptableObject {
        // Ele é um singleton acessado por funções estáticas
        private static AssetManager instance = null;
        private static AssetManager Instance {
            get {
                if (instance == null) {
                    instance = StaticReferences.AssetManager;
                }
                return instance;
            }
        }

        [SerializeField] private AssetManagerBundle<Party> party = new AssetManagerBundle<Party>();
        [SerializeField] private AssetManagerBundle<Hero> heroes = new AssetManagerBundle<Hero>();
        [SerializeField] private AssetManagerBundle<Enemy> enemies = new AssetManagerBundle<Enemy>();
        [SerializeField] private AssetManagerBundle<Skill> skills = new AssetManagerBundle<Skill>();
        [SerializeField] private AssetManagerBundle<Quest> quests = new AssetManagerBundle<Quest>();

        private AssetManagerBundle<T> GetBundle<T>(string typeName) where T : ScriptableObject, IDatabaseItem {
            switch (typeName) {
                case "party":
                    return party as AssetManagerBundle<T>;
                case "hero":
                    return heroes as AssetManagerBundle<T>;
                case "enemy":
                    return enemies as AssetManagerBundle<T>;
                case "skill":
                    return skills as AssetManagerBundle<T>;
                case "quest":
                    return quests as AssetManagerBundle<T>;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    return null;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Build")]
        public void BuildDatabase() {
            InitializeBundles();

            if (!Application.isPlaying) {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }

        private void InitializeBundles() {
            party = new AssetManagerBundle<Party>();
            party.InitializeAssets();
            heroes = new AssetManagerBundle<Hero>();
            heroes.InitializeAssets();
            enemies = new AssetManagerBundle<Enemy>();
            enemies.InitializeAssets();
            skills = new AssetManagerBundle<Skill>();
            skills.InitializeAssets();
            quests = new AssetManagerBundle<Quest>();
            quests.InitializeAssets();
        }
#endif

        public static void PreloadBundles() {
            if (Instance == null) {
                Debug.LogError("No database to preload");
                return;
            }

            PreloadAssets();
        }

        private static void PreloadAssets() {
            Instance.party.PreloadAssets();
            Instance.heroes.PreloadAssets();
            Instance.enemies.PreloadAssets();
            Instance.skills.PreloadAssets();
            Instance.quests.PreloadAssets();
        }

        public static ScriptableObject LoadAsset(string name, System.Type type) {
            string typeName = type.Name.ToLower();
            if (!IsTypeSupported(type)) {
                Debug.Log($"Access to bundle {typeName} is not implemented");
                return null;
            }

            return LoadAssetByNameAndType(name, typeName);
        }

        public static bool IsTypeSupported(System.Type type) {
            string typeName = type.Name.ToLower();
            return typeName switch {
                "party" => true,
                "hero" => true,
                "enemy" => true,
                "skill" => true,
                "quest" => true,
                _ => false,
            };
        }

        private static ScriptableObject LoadAssetByNameAndType(string name, string type) {
            return type switch {
                "party" => LoadAsset<Party>(name),
                "hero" => LoadAsset<Hero>(name),
                "enemy" => LoadAsset<Enemy>(name),
                "skill" => LoadAsset<Skill>(name),
                "quest" => LoadAsset<Quest>(name),
                _ => throw new System.NotImplementedException(),
            };
        }

        public static T LoadAsset<T>(string name, string typeName = null) where T : ScriptableObject, IDatabaseItem {
            if (Instance == null) {
                Debug.LogError("Database has not been loaded");
                return default;
            }

            typeName ??= typeof(T).Name.ToLower();
            AssetManagerBundle<T> bundle = Instance.GetBundle<T>(typeName);
            return bundle?.GetAsset(name);
        }

        public static T[] LoadAllAssets<T>(string typeName = null) where T : ScriptableObject, IDatabaseItem {
            if (Instance == null) {
                Debug.LogError("Database has not been loaded");
                return default;
            }

            typeName ??= typeof(T).Name.ToLower();
            AssetManagerBundle<T> bundle = Instance.GetBundle<T>(typeName);
            return bundle?.GetAllAssets();
        }
    }
}
