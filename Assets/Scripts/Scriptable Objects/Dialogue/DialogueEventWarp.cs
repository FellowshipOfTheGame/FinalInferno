using UnityEngine;
using FinalInferno.EventSystem;
using Fog.Dialogue;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewEventWarpDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueEventWarp")]
    public class DialogueEventWarp : DialogueEventTrigger, IUpdatableScript {
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

        public void UpdateThisObject() {
            sceneChangeInfo = new SceneChangeInfo();
            sceneChangeInfo.scene = !string.IsNullOrEmpty(scene.scene) ? new ScenePicker(scene.scene) : new ScenePicker();
            sceneChangeInfo.destinationPosition = scene.position;
            sceneChangeInfo.isCutscene = false;
            sceneChangeInfo.cutsceneDialogue = null;
            sceneChangeInfo.savePosition = Vector2.zero;
            string guid = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(SceneChangeInfoReference)}")[0];
            sceneChangeInfoReference = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneChangeInfoReference>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
            guid = UnityEditor.AssetDatabase.FindAssets($"Start Scene Change t:{typeof(EventFI)}")[0];
            startSceneChangeAnimation = UnityEditor.AssetDatabase.LoadAssetAtPath<EventFI>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}