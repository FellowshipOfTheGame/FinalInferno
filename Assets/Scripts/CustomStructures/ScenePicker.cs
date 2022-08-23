﻿using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class ScenePicker {
        [SerializeField] private string sceneName = "";
        [SerializeField] private string assetPath = "";
        [SerializeField] private string guid = "";
        public string Name { get => sceneName; private set => sceneName = value; }
        public string Path { get => assetPath; private set => assetPath = value; }
        public string GUID { get => guid; private set => guid = value; }

        public ScenePicker(string sceneName) {
            Name = sceneName;
            guid = UnityEditor.AssetDatabase.FindAssets($"{sceneName} t:SceneAsset")[0];
            Path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
        }

        public ScenePicker() {
            sceneName = "";
            assetPath = "";
            guid = "";
        }

        public ScenePicker(ScenePicker other) {
            CopyValues(other);
        }

        public void CopyValues(ScenePicker other) {
            sceneName = other.sceneName;
            assetPath = other.assetPath;
            guid = other.guid;
        }
    }
}