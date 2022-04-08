using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewEventWarpDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueEventWarp")]
    public class DialogueEventWarp : DialogueEventTrigger {
        // TO DO: adicionar suporte de cutscene aqui
        [Header("Scene")]
        [SerializeField] private SceneWarp scene;
        [Header("Expected value = TriggerChangeScene")]
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        public override void AfterDialogue() {
            if (string.IsNullOrEmpty(scene.scene) || decision == null) {
                IgnoreSceneChange();
            } else {
                PrepareSceneChange();
                ChangeScene();
            }
            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }

        private void IgnoreSceneChange() {
            shouldUnlockMovement = true;
            base.AfterDialogue();
        }

        private void PrepareSceneChange() {
            SceneLoader.beforeSceneChange += SetFlags;
            shouldUnlockMovement = false;
        }

        public void SetFlags() {
            base.AfterDialogue();
            SceneLoader.beforeSceneChange -= SetFlags;
        }

        private void ChangeScene() {
            FinalInferno.UI.ChangeSceneUI.sceneName = scene.scene;
            FinalInferno.UI.ChangeSceneUI.positionOnLoad = scene.position;
            FinalInferno.UI.ChangeSceneUI.isCutscene = false;
            FinalInferno.UI.ChangeSceneUI.selectedDialogue = null;
            decision.Click();
        }
    }
}