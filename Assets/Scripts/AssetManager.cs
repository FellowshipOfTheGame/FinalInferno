using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace FinalInferno{
    public static class AssetManager
    {
        private static List<AssetBundle> bundleList = new List<AssetBundle>();

        private static AssetBundle party = null;
        private static AssetBundle Party{
            get{
                if(party == null || !bundleList.Contains(party)){
                    party = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "party"));
                    bundleList.Add(party);
                }
                return party;
            }
        }
        private static AssetBundle hero = null;
        private static AssetBundle Hero{
            get{
                if(hero == null || !bundleList.Contains(hero)){
                    hero = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "hero"));
                    bundleList.Add(hero);
                }
                return hero;
            }
        }
        private static AssetBundle character = null;
        private static AssetBundle Character{
            get{
                if(character == null || !bundleList.Contains(character)){
                    character = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "character"));
                    bundleList.Add(character);
                }
                return character;
            }
        }
        private static AssetBundle enemy = null;
        private static AssetBundle Enemy{
            get{
                if(enemy == null || !bundleList.Contains(enemy)){
                    enemy = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "enemy"));
                    bundleList.Add(enemy);
                }
                return enemy;
            }
        }
        private static AssetBundle skill = null;
        private static AssetBundle Skill{
            get{
                if(skill == null || !bundleList.Contains(skill)){
                    skill = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "skill"));
                    bundleList.Add(skill);
                }
                return skill;
            }
        }

        public static void LoadAllBundles(){
            if(Party && Character && Hero && Enemy && Skill)
            return;
        }

        public static void LoadAllAssets(){
            LoadAllBundles();
            if(Party)
                LoadBundleAssets<Party>();
            if(Character)
                LoadBundleAssets<Character>();
            if(Hero)
                LoadBundleAssets<Hero>();
            if(Enemy)
                LoadBundleAssets<Enemy>();
            if(Skill)
                LoadBundleAssets<Skill>();
            return;
        }

        private static AssetBundle GetBundle(string typeName){
            AssetBundle bundle = null;
            switch(typeName){
                case "party":
                    bundle = Party;
                    break;
                case "hero":
                    bundle = Hero;
                    break;
                case "character":
                    bundle = Character;
                    break;
                case "enemy":
                    bundle = Enemy;
                    break;
                case "skill":
                    bundle = Skill;
                    break;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    break;
            }
            return bundle;
        }

        public static List<T> LoadBundleAssets<T>() where T : UnityEngine.Object{
            #if UNITY_EDITOR
            string[] objectsFound = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            List<T> newList = new List<T>();
            foreach(string guid in objectsFound){
                newList.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
            }
            return newList;
            #endif
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : new List<T>(bundle.LoadAllAssets<T>());
        }

        public static T LoadAsset<T>(string name) where T : UnityEngine.Object{
            Debug.Log("looking for object " + name + " of type " + typeof(T).Name);
            #if UNITY_EDITOR
            string[] objectsFound = UnityEditor.AssetDatabase.FindAssets(name + " t:" + typeof(T).Name);
            if(objectsFound != null && objectsFound.Length > 0){
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(objectsFound[0]));
            }else{
                Debug.Log("object " + name + " not found");
                return null;
            }
            #endif
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : bundle.LoadAsset<T>(name);
        }

        public static UnityEngine.Object LoadAsset(string name, System.Type type){
            Debug.Log("looking for object " + name + " of type " + type.Name);
            #if UNITY_EDITOR
            string[] objectsFound = UnityEditor.AssetDatabase.FindAssets(name + " t:" + type.Name);
            if(objectsFound != null && objectsFound.Length > 0){
                return UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(objectsFound[0]), type);
            }else{
                Debug.Log("object " + name + " not found");
                return null;
            }
            #endif
            string typeName = type.Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : bundle.LoadAsset(name, type);
        }

        public static void UnloadAssets<T>(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #endif
            
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            bundle.Unload(shouldDestroy);
            bundleList.Remove(bundle);
        }

        public static void UnloadAllAssets(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #endif
            foreach(AssetBundle bundle in bundleList){
                bundle.Unload(shouldDestroy);
            }
            bundleList.Clear();
        }
    }
}
