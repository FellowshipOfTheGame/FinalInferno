using System.Collections;
using UnityEngine;

namespace FinalInferno {
    public class LoadStuff : MonoBehaviour {
        [SerializeField] private bool demo = false;
        [SerializeField] private VolumeController volumeController = null;

        private void Start() {
            Cursor.visible = false;
            volumeController.ResetValues();
            SaveLoader.AutoSave = true;

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