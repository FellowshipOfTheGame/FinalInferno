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
            Debug.Log("teste");
            if (scene.scene == null || scene.scene == "" || decision == null) {
                shouldUnlockMovement = true;
                base.AfterDialogue();
            } else {
                SceneLoader.beforeSceneChange += SetFlags;
                shouldUnlockMovement = false;

                FinalInferno.UI.ChangeSceneUI.sceneName = scene.scene;
                FinalInferno.UI.ChangeSceneUI.positionOnLoad = scene.position;
                FinalInferno.UI.ChangeSceneUI.isCutscene = false;
                FinalInferno.UI.ChangeSceneUI.selectedDialogue = null;

                decision.Click();
            }
            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }

        public void SetFlags() {
            base.AfterDialogue();
            SceneLoader.beforeSceneChange -= SetFlags;
        }
    }
}