using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace FinalInferno{
    public static class AssetManager
    {
        private static AssetBundle party = null;
        private static AssetBundle Party{
            get{
                if(party == null){
                    party = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "party"));
                }
                return party;
            }
        }
        private static AssetBundle hero = null;
        private static AssetBundle Hero{
            get{
                if(hero == null){
                    hero = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "hero"));
                }
                return hero;
            }
        }
        private static AssetBundle character = null;
        private static AssetBundle Character{
            get{
                if(character == null){
                    character = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "character"));
                }
                return character;
            }
        }
        private static AssetBundle enemy = null;
        private static AssetBundle Enemy{
            get{
                if(enemy == null){
                    enemy = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "enemy"));
                }
                return enemy;
            }
        }
        private static AssetBundle skill = null;
        private static AssetBundle Skill{
            get{
                if(skill == null){
                    skill = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "skill"));
                }
                return skill;
            }
        }

        public static void LoadAllAssets(){
            if(Party && Character && Hero && Skill && Enemy)
            return;
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
            AssetBundle bundle = null;
            switch(typeName){
                case "party":
                    bundle = Party;
                    break;
                case "hero":
                    bundle = Hero;
                    break;
                case "enemy":
                    bundle = Enemy;
                    break;
                case "skill":
                    bundle = Skill;
                    break;
                case "character":
                    bundle = Character;
                    break;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    return null;
            }
            return bundle.LoadAsset<T>(name);
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
            AssetBundle bundle = null;
            switch(typeName){
                case "party":
                    bundle = Party;
                    break;
                case "hero":
                    bundle = Hero;
                    break;
                case "enemy":
                    bundle = Enemy;
                    break;
                case "skill":
                    bundle = Skill;
                    break;
                case "character":
                    bundle = Character;
                    break;
                default:
                    Debug.Log("Access to bundle " + typeName + " is not implemented");
                    return null;
            }
            return bundle.LoadAsset(name, type);
        }

        public static void UnloadAssets<T>(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #endif
            
            switch(typeof(T).ToString().ToLower()){
                case "party":
                    party.Unload(shouldDestroy);
                    party = null;
                    break;
                case "enemy":
                    enemy.Unload(shouldDestroy);
                    enemy = null;
                    break;
                case "skill":
                    skill.Unload(shouldDestroy);
                    skill = null;
                    break;
                default:
                    Debug.Log("Access to bundle " + typeof(T).ToString().ToLower() + " is not implemented");
                    return;

            }
        }

        public static void UnloadAllAssets(bool shouldDestroy = true){
            #if UNITY_EDITOR
            return;
            #endif
            AssetBundle.UnloadAllAssetBundles(shouldDestroy);
            party = null;
            hero = null;
            enemy = null;
            character = null;
            skill = null;
        }
    }
}
