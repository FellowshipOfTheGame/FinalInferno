using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class Bundle<T> where T : ScriptableObject, IDatabaseItem {
        // Lista serializavel configurada pelo editor
        // TO DO: Adicionar HideInInspector depois que tiver certeza que funciona
        [SerializeField] private List<T> assets = new List<T>();

        private string bundleName = "";
        public string BundleName => bundleName;

        private Dictionary<string, T> dict = new Dictionary<string, T>();
        private bool loaded = false;

        public Bundle() {
            bundleName = typeof(T).Name.ToLower();
        }

#if UNITY_EDITOR
        public void InitializeAsset() {
            string[] guidAssetsPath = LocateAssets();
            LoadAssets(guidAssetsPath);

            if (!Application.isPlaying) {
                AssetDatabase.SaveAssets();
            }
        }

        private string[] LocateAssets() {
            return AssetDatabase.FindAssets("t:" + typeof(T).Name);
        }

        private void LoadAssets(string[] guidAssetsPath) {
            assets = new List<T>();
            foreach (string guidPath in guidAssetsPath) {
                T newAsset = CreateLoadedAsset(AssetDatabase.GUIDToAssetPath(guidPath));
                assets.Add(newAsset);
            }
        }

        private T CreateLoadedAsset(string assetPath) {
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            asset.LoadTables();
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(asset);
            }
            return asset;
        }
#endif

        public void PreloadAsset() {
            foreach (T asset in assets) {
                string key = GetAssetKey(asset);

                try {
                    dict.Add(key, asset);
                    asset.Preload();
                } catch (System.ArgumentException) {
                    Debug.LogWarning($"Asset {key} is being added more than once");
                }
            }
        }

        private string GetAssetKey(T asset) {
            return (asset is Enemy) ? (asset as Enemy).AssetName : asset.name;
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
