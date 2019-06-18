using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace FinalInferno{
    public static class AssetManager
    {
        private static List<AssetBundle> bundleList;
        private static List<AssetBundle> BundleList {
            get{
                if(bundleList == null)
                    bundleList = new List<AssetBundle>();
                return bundleList;
            }
        }

        private static AssetBundle party = null;
        private static AssetBundle Party{
            get{
                if(party == null || !BundleList.Contains(party)){
                    party = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "party"));
                    BundleList.Add(party);
                }
                return party;
            }
        }
        private static AssetBundle hero = null;
        private static AssetBundle Hero{
            get{
                if(hero == null || !BundleList.Contains(hero)){
                    hero = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "hero"));
                    BundleList.Add(hero);
                }
                return hero;
            }
        }
        private static AssetBundle character = null;
        private static AssetBundle Character{
            get{
                if(character == null || !BundleList.Contains(character)){
                    character = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "character"));
                    BundleList.Add(character);
                }
                return character;
            }
        }
        private static AssetBundle enemy = null;
        private static AssetBundle Enemy{
            get{
                if(enemy == null || !BundleList.Contains(enemy)){
                    enemy = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "enemy"));
                    BundleList.Add(enemy);
                }
                return enemy;
            }
        }
        private static AssetBundle skill = null;
        private static AssetBundle Skill{
            get{
                if(skill == null || !BundleList.Contains(skill)){
                    skill = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "skill"));
                    BundleList.Add(skill);
                }
                return skill;
            }
        }
        private static AssetBundle quest = null;
        private static AssetBundle Quest{
            get{
                if(quest == null || !BundleList.Contains(skill)){
                    quest = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "quest"));
                    BundleList.Add(quest);
                }
                return quest;
            }
        }

        public static void LoadAllBundles(){
            #if UNITY_EDITOR
            return;
            #else
            if(Party && Character && Hero && Enemy && Skill && Quest)
            return;
            #endif
        }

        public static void LoadAllAssets(){
            #if UNITY_EDITOR
            return;
            #else
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
            if(Quest)
                LoadBundleAssets<Quest>();
            return;
            #endif
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
                case "quest":
                    bundle = Quest;
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
            #else
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : new List<T>(bundle.LoadAllAssets<T>());
            #endif
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
            #else
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : bundle.LoadAsset<T>(name);
            #endif
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
            #else
            string typeName = type.Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            return (bundle == null)? null : bundle.LoadAsset(name, type);
            #endif
        }

        public static void UnloadAssets<T>(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #else
            
            string typeName = typeof(T).Name.ToLower();
            AssetBundle bundle = GetBundle(typeName);
            bundle.Unload(shouldDestroy);
            BundleList.Remove(bundle);
            #endif
        }

        public static void UnloadAllAssets(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #else
            foreach(AssetBundle bundle in BundleList){
                bundle.Unload(shouldDestroy);
            }
            BundleList.Clear();
            #endif
        }
    }
}
