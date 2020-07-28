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
        [System.Serializable]
        private class Bundle<T> where T : ScriptableObject{
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
                string[] objectsFound = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                assets = new List<T>();
                foreach(string guid in objectsFound){
                    assets.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
                }
                foreach(T obj in assets){
                    Debug.Log(obj.name);
                }
            }
            #endif

            public void Preload(){
                // if(!loaded){
                    loaded = true;
                    foreach(T asset in assets){
                        string key = (asset is Enemy)? (asset as Enemy).AssetName : asset.name;
                        try{
                            dict.Add(key, asset);
                        }catch(System.ArgumentException){
                            Debug.LogWarning("Asset " + key + " is being added more than once");
                        }
                    }
                // }
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
        [System.Serializable] private class PartyBundle : Bundle<Party>{}
        [System.Serializable] private class HeroBundle : Bundle<Hero>{}
        [System.Serializable] private class CharacterBundle : Bundle<Character>{}
        [System.Serializable] private class EnemyBundle : Bundle<Enemy>{}
        [System.Serializable] private class SkillBundle : Bundle<Skill>{}
        [System.Serializable] private class QuestBundle : Bundle<Quest>{}
        // Fim da subclasse =====================================================================

        // Unity não consegue serializar isso, oh well
        // [SerializeField] private Bundle<Party> party = new Bundle<Party>();
        // [SerializeField] private Bundle<Hero> heroes = new Bundle<Hero>();
        // [SerializeField] private Bundle<Character> characters = new Bundle<Character>();
        // [SerializeField] private Bundle<Enemy> enemies = new Bundle<Enemy>();
        // [SerializeField] private Bundle<Skill> skills = new Bundle<Skill>();
        // [SerializeField] private Bundle<Quest> quests = new Bundle<Quest>();

        [SerializeField] private PartyBundle party = new PartyBundle();
        [SerializeField] private HeroBundle heroes = new HeroBundle();
        [SerializeField] private CharacterBundle characters = new CharacterBundle();
        [SerializeField] private EnemyBundle enemies = new EnemyBundle();
        [SerializeField] private SkillBundle skills = new SkillBundle();
        [SerializeField] private QuestBundle quests = new QuestBundle();

        private Bundle<T> GetBundle<T>(string typeName) where T : ScriptableObject{
            Bundle<T> bundle = null;
            switch(typeName){
                case "party":
                    bundle = party as Bundle<T>;
                    break;
                case "hero":
                    bundle = heroes as Bundle<T>;
                    break;
                case "character":
                    bundle = characters as Bundle<T>;
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
            // party = new Bundle<Party>();
            party = new PartyBundle();
            party.FindAssets();
            // heroes = new Bundle<Hero>();
            heroes = new HeroBundle();
            heroes.FindAssets();
            // characters = new Bundle<Character>();
            characters = new CharacterBundle();
            characters.FindAssets();
            // enemies = new Bundle<Enemy>();
            enemies = new EnemyBundle();
            enemies.FindAssets();
            // skills = new Bundle<Skill>();
            skills = new SkillBundle();
            skills.FindAssets();
            // quests = new Bundle<Quest>();
            quests = new QuestBundle();
            quests.FindAssets();
        }
        #endif

        public static void Preload(){
            if(Instance != null){
                Instance.party.Preload();
                Instance.heroes.Preload();
                Instance.characters.Preload();
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
                case "character":
                    return LoadAsset<Character>(name);
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
            // if(type == typeof(ScriptableObject) || type.IsSubclassOf(typeof(ScriptableObject))){
            //     return LoadAsset<ScriptableObject>(name, type.Name.ToLower());
            // }else{
            //     return null;
            // }
        }

        public static T LoadAsset<T>(string name, string typeName = null) where T : ScriptableObject{
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
