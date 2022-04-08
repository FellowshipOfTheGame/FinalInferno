using System.Collections;
using Fog.Dialogue;
using UnityEngine;

namespace FinalInferno {
    public class LoadStuff : MonoBehaviour {
        [SerializeField] private bool demo = false;
        [SerializeField] private VolumeController volumeController = null;

        private void Start() {
#if !UNITY_EDITOR
            Cursor.visible = false;
#endif
            volumeController.ResetValues();
            SaveLoader.AutoSave = true;
            DialogueHandler.debugActivated = StaticReferences.DebugBuild;

            StartCoroutine(PreloadAsync());
        }

        private IEnumerator PreloadAsync() {
            yield return new WaitForEndOfFrame();
#if UNITY_EDITOR
            // No editor recarrega as tabelas por precaução
            StaticReferences.AssetManager.BuildDatabase();
#endif
            AssetManager.Preload();
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForEndOfFrame();
            LoadFirstScene();
        }

        private void LoadFirstScene() {
            if (demo) {
                SaveLoader.StartDemo();
            } else {
                SceneLoader.LoadMainMenu();
            }
        }
    }
}