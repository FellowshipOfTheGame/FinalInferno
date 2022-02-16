using System.Collections.Generic;
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

        [SerializeField] private Bundle<Party> party = new Bundle<Party>();
        [SerializeField] private Bundle<Hero> heroes = new Bundle<Hero>();
        [SerializeField] private Bundle<Enemy> enemies = new Bundle<Enemy>();
        [SerializeField] private Bundle<Skill> skills = new Bundle<Skill>();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        [SerializeField] private Bundle<Quest> quests = new Bundle<Quest>();

        private Bundle<T> GetBundle<T>(string typeName) where T : ScriptableObject, IDatabaseItem {
            switch (typeName) {
                case "party":
                    return party as Bundle<T>;
                case "hero":
                    return heroes as Bundle<T>;
                case "enemy":
                    return enemies as Bundle<T>;
                case "skill":
                    return skills as Bundle<T>;
                case "quest":
                    return quests as Bundle<T>;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    return null;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Build")]
        public void BuildDatabase() {
            InitializeBundles();

            EditorUtility.SetDirty(this);

            if (!Application.isPlaying) {
                AssetDatabase.SaveAssets();
            }
        }

        private void InitializeBundles() {
            party = new Bundle<Party>();
            party.InitializeAsset();
            heroes = new Bundle<Hero>();
            heroes.InitializeAsset();
            enemies = new Bundle<Enemy>();
            enemies.InitializeAsset();
            skills = new Bundle<Skill>();
            skills.InitializeAsset();
            quests = new Bundle<Quest>();
            quests.InitializeAsset();
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
            Instance.party.PreloadAsset();
            Instance.heroes.PreloadAsset();
            Instance.enemies.PreloadAsset();
            Instance.skills.PreloadAsset();
            Instance.quests.PreloadAsset();
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

        private static ScriptableObject LoadAssetByNameAndType(string name, string type){
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
            Debug.Log($"looking for object {name} of type {typeName} as {typeof(T).Name}");
            Bundle<T> bundle = Instance.GetBundle<T>(typeName);
            return bundle?.GetAsset(name);
        }
    }
}
