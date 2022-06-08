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

        // Subclasse =====================================================================
        #region subclass
        [System.Serializable]
        private class Bundle<T> where T : ScriptableObject, IDatabaseItem {
            // Lista serializavel configurada pelo editor
            // TO DO: Adicionar HideInInspector depois que tiver certeza que funciona
            [SerializeField] private List<T> assets = new List<T>();
            // Talvez isso seja desnecessario
            [SerializeField, HideInInspector] private string bundleName = "";
            public string BundleName => bundleName;
            // Dicionario carregado em runtime para acesso rapido
            private Dictionary<string, T> dict = new Dictionary<string, T>();

            public Bundle() {
                bundleName = typeof(T).Name.ToLower();
            }

#if UNITY_EDITOR
            public void FindAssets() {
                string[] objectsFound = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                assets = new List<T>();
                foreach (string guid in objectsFound) {
                    T newAsset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                    newAsset.LoadTables();
                    if (!Application.isPlaying) {
                        newAsset.Preload();
                        EditorUtility.SetDirty(newAsset);
                    }
                    assets.Add(newAsset);
                }
                if (!Application.isPlaying) {
                    AssetDatabase.SaveAssets();
                }
            }
#endif

            public void Preload() {
                foreach (T asset in assets) {
                    string key = (asset is Enemy) ? (asset as Enemy).AssetName : asset.name;
                    try {
                        dict.Add(key, asset);
                        asset.Preload();
                    } catch (System.ArgumentException) {
                        Debug.LogWarning($"Asset {key} is being added more than once");
                    }
                }
            }

            public T LoadAsset(string name) {
                try {
                    return dict[name];
                } catch (KeyNotFoundException) {
                    return default;
                }
            }
        }
        // Fim da subclasse =====================================================================
        #endregion

        // Unity não consegue serializar isso, oh well
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
            party = new Bundle<Party>();
            party.FindAssets();
            heroes = new Bundle<Hero>();
            heroes.FindAssets();
            enemies = new Bundle<Enemy>();
            enemies.FindAssets();
            skills = new Bundle<Skill>();
            skills.FindAssets();
            quests = new Bundle<Quest>();
            quests.FindAssets();
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }
#endif

        public static void Preload() {
            if (Instance == null) {
                Debug.LogError("No database to preload");
                return;
            }
            Instance.party.Preload();
            Instance.heroes.Preload();
            Instance.enemies.Preload();
            Instance.skills.Preload();
            Instance.quests.Preload();
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

        public static ScriptableObject LoadAsset(string name, System.Type type) {
            string typeName = type.Name.ToLower();
            if (!IsTypeSupported(type)) {
                Debug.Log($"Access to bundle {typeName} is not implemented");
                return null;
            }

            return typeName switch {
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
            return bundle?.LoadAsset(name);
        }
    }
}
