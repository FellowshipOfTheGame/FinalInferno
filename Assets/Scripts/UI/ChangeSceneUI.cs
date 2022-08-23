using UnityEngine;
using Fog.Dialogue;
using FinalInferno.EventSystem;

namespace FinalInferno.UI {
    public class ChangeSceneUI : MonoBehaviour, IVariableObserver<bool> {
        private const string IsBattleAnimString = "IsBattle";
        private Animator anim;
        private bool hasIsBattleParameter;
        [SerializeField] private LoadEnemiesPreview loadEnemiesPreview;
        [SerializeField] BoolVariable isLoadingBattle;
        [SerializeField] VoidEventListenerFI startSceneChangeAnimationListener;
        [SerializeField] private BattleInfoReference battleInfo;
        [SerializeField] private SceneChangeInfoReference sceneChangeInfo;

        private void Awake() {
            anim = GetComponent<Animator>();
            hasIsBattleParameter = System.Array.Find(anim.parameters, parameter => parameter.name == IsBattleAnimString) != null;
            isLoadingBattle.UpdateValue(true);
        }

        private void OnEnable() {
            startSceneChangeAnimationListener.StartListeningEvent();
            isLoadingBattle.AddObserver(this);
        }

        private void OnDisable() {
            isLoadingBattle.RemoveObserver(this);
            startSceneChangeAnimationListener.StopListeningEvent();
        }

        private void MainMenu() {
            SceneLoader.LoadMainMenu();
        }

        #region Overworld Callbacks
        private void ChangeMap() {
            sceneChangeInfo.LoadSceneFromMenuOrOW();
        }

        public void SceneLoadCallback() {
            SceneLoader.onSceneLoad?.Invoke();
        }

        private void LoadPreview() {
            loadEnemiesPreview.LoadPreview();
        }

        private void StartBattle() {
            isLoadingBattle.UpdateValue(false);
            SceneLoader.LoadBattleScene(battleInfo);
        }
        #endregion

        #region Battle Callbacks
        private void ReturnCheckpoint() {
            SaveLoader.LoadGame();
        }

        private void Continue() {
            sceneChangeInfo.LoadSceneFromBattle();
        }

        public void ValueChanged(bool value) {
            if (hasIsBattleParameter)
                anim.SetBool(IsBattleAnimString, value);
        }
        #endregion
    }

}