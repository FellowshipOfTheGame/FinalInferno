using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "AssetDatabase", menuName = "ScriptableObject/Database")]
    public class AssetManager : ScriptableObject
    {
        // Ele é um singleton acessado por funções estáticas
        private static AssetManager instance = null;
        private static AssetManager Instance {
            get{
                if(instance == null){
                    instance = StaticReferences.AssetManager;
                }
                return instance;
            }
        }

        // Subclasse =====================================================================
        #region subclass
        [System.Serializable]
        private class Bundle<T> where T : ScriptableObject, IDatabaseItem{
            // Lista serializavel configurada pelo editor
            // TO DO: Adicionar HideInInspector depois que tiver certeza que funciona
            [SerializeField] private List<T> assets = new List<T>();
            // Talvez isso seja desnecessario
            [SerializeField, HideInInspector] private string bundleName = "";
            public string BundleName { get => bundleName; }
            // Dicionario carregado em runtime para acesso rapido
            private Dictionary<string, T> dict = new Dictionary<string, T>();
            private bool loaded = false;

            public Bundle(){
                bundleName = typeof(T).Name.ToLower();
            }

            #if UNITY_EDITOR
            public void FindAssets(){
                string[] objectsFound = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                assets = new List<T>();
                foreach(string guid in objectsFound){
                    T newAsset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                    newAsset.LoadTables();
                    if(!Application.isPlaying){
                        newAsset.Preload();
                        EditorUtility.SetDirty(newAsset);
                    }
                    assets.Add(newAsset);
                }
                if(!Application.isPlaying){
                    AssetDatabase.SaveAssets();
                }
            }
            #endif

            public void Preload(){
                foreach(T asset in assets){
                    string key = (asset is Enemy)? (asset as Enemy).AssetName : asset.name;
                    try{
                        dict.Add(key, asset);
                        // Codigo aqui embaixo só executa se não cair no catch
                        asset.Preload();
                    }catch(System.ArgumentException){
                        Debug.LogWarning("Asset " + key + " is being added more than once");
                    }
                }
            }

            public T LoadAsset(string name){
                T value = default(T);
                try{
                    value = dict[name];
                }catch(KeyNotFoundException){
                    value = default(T);
                }
                // O uso do dicionario é mais eficiente (~= O(1)) do que um find na lista (O(n))
                // value = assets.Find(asset => (asset is Enemy)? ((asset as Enemy).AssetName == name) : (asset.name == name));
                return value;
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

        private Bundle<T> GetBundle<T>(string typeName) where T : ScriptableObject, IDatabaseItem{
            Bundle<T> bundle = null;
            switch(typeName){
                case "party":
                    bundle = party as Bundle<T>;
                    break;
                case "hero":
                    bundle = heroes as Bundle<T>;
                    break;
                case "enemy":
                    bundle = enemies as Bundle<T>;
                    break;
                case "skill":
                    bundle = skills as Bundle<T>;
                    break;
                case "quest":
                    bundle = quests as Bundle<T>;
                    break;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    break;
            }

            return bundle;
        }

        #if UNITY_EDITOR
        [ContextMenu("Build")]
        public void BuildDatabase(){
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
            if(!Application.isPlaying){
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }
        #endif

        public static void Preload(){
            if(Instance != null){
                Instance.party.Preload();
                Instance.heroes.Preload();
                Instance.enemies.Preload();
                Instance.skills.Preload();
                Instance.quests.Preload();
            }else{
                Debug.LogError("No database to preload");
            }
        }

        public static ScriptableObject LoadAsset(string name, System.Type type){
            string typeName = type.Name.ToLower();
            switch(typeName){
                case "party":
                    return LoadAsset<Party>(name);
                case "hero":
                    return LoadAsset<Hero>(name);
                case "enemy":
                    return LoadAsset<Enemy>(name);
                case "skill":
                    return LoadAsset<Skill>(name);
                case "quest":
                    return LoadAsset<Quest>(name);
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    return null;
            }
        }

        public static T LoadAsset<T>(string name, string typeName = null) where T : ScriptableObject, IDatabaseItem{
            if(Instance != null){
                if(typeName == null){
                    typeName = typeof(T).Name.ToLower();
                }
                Debug.Log("looking for object " + name + " of type " + typeName + " as " + typeof(T).Name);
                Bundle<T> bundle = Instance.GetBundle<T>(typeName);
                return (bundle == null)? null : bundle.LoadAsset(name);
            }else{
                Debug.LogError("Database has not been loaded");
                return default(T);
            }
        }
    }
}
