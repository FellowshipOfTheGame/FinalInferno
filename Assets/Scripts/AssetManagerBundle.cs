using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class AssetManagerBundle<T> where T : ScriptableObject, IDatabaseItem {
        // Lista serializavel configurada pelo editor
        // TO DO: Adicionar HideInInspector depois que tiver certeza que funciona
        [SerializeField] private List<T> assets = new List<T>();
        private Dictionary<string, T> dict = new Dictionary<string, T>();

#if UNITY_EDITOR
        public void InitializeAssets() {
            string[] assetsGUIDs = LocateAssets();
            LoadAssets(assetsGUIDs);

            if (!Application.isPlaying) {
                AssetDatabase.SaveAssets();
            }
        }

        private string[] LocateAssets() {
            return AssetDatabase.FindAssets("t:" + typeof(T).Name);
        }

        private void LoadAssets(string[] assetsGUIDs) {
            assets = new List<T>();
            foreach (string assetGUID in assetsGUIDs) {
                T newAsset = GetLoadedAsset(AssetDatabase.GUIDToAssetPath(assetGUID));
                assets.Add(newAsset);
            }
        }

        private T GetLoadedAsset(string assetPath) {
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            asset.LoadTables();
            if (!Application.isPlaying) {
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(asset);
            }
            return asset;
        }
#endif

        public void PreloadAssets() {
            foreach (T asset in assets) {
                string key = GetAssetKey(asset);
                TryPreloadAsset(key, asset);
            }
        }

        private string GetAssetKey(T asset) {
            return (asset is Enemy) ? (asset as Enemy).AssetName : asset.name;
        }

        private void TryPreloadAsset(string key, T asset) {
            try {
                dict.Add(key, asset);
                asset.Preload();
            } catch (System.ArgumentException) {
                Debug.LogWarning($"Asset {key} is being added more than once");
            }
        }

        public T GetAsset(string key) {
            try {
                return dict[key];
            } catch (KeyNotFoundException) {
                return default;
            }
        }
    }
}
