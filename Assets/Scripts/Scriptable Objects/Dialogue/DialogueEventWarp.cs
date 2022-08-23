using UnityEngine;
using FinalInferno.EventSystem;
using Fog.Dialogue;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewEventWarpDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueEventWarp")]
    public class DialogueEventWarp : DialogueEventTrigger {
        [Header("Scene")]
        [SerializeField] private SceneWarp scene;
        [SerializeField] private SceneChangeInfo sceneChangeInfo = new SceneChangeInfo();
        [SerializeField] private SceneChangeInfoReference sceneChangeInfoReference;
        [SerializeField] private EventFI startSceneChangeAnimation;

        public override void AfterDialogue() {
            if (string.IsNullOrEmpty(sceneChangeInfo.scene.Name) || startSceneChangeAnimation == null) {
                IgnoreSceneChange();
            } else {
                PrepareSceneChange();
                ChangeScene();
            }
            DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
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
            sceneChangeInfoReference.SetValues(sceneChangeInfo);
            startSceneChangeAnimation.Raise();
        }
    }
}