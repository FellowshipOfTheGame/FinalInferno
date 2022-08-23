using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewBattleDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueBeforeBattle")]
    public class DialogueBeforeBattle : DialogueEventTrigger {
        [SerializeField] private BattleInfo battleInfo;
        [Header("Scene Change")]
        [SerializeField] private BoolVariable isLoadingBattle;
        [SerializeField] private EventFI startSceneChangeAnimation;
        [SerializeField] private BattleInfoReference battleInfoReference;
        [SerializeField] private SceneChangeInfoReference sceneChangeInfoReference;
        [Header("Optional")]
        [SerializeField] private DialogueFI dialogueAfterBattle;

        public override void AfterDialogue() {
            PrepareSceneChange();
            StartBattle();
            Fog.Dialogue.DialogueHandler.instance.OnDialogueEnd -= AfterDialogue;
        }

        private void PrepareSceneChange() {
            shouldUnlockMovement = false;
            SceneLoader.beforeSceneChange += SetFlags;
            SceneLoader.CutsceneDialogue = dialogueAfterBattle;
        }

        public void SetFlags() {
            base.AfterDialogue();
            SceneLoader.beforeSceneChange -= SetFlags;
        }

        private void StartBattle() {
            isLoadingBattle.UpdateValue(true);
            SceneChangeInfo sceneChangeInfo = CreateSceneChangeInfo();
            sceneChangeInfoReference.SetValues(sceneChangeInfo);
            battleInfoReference.SetValues(battleInfo);
            startSceneChangeAnimation.Raise();
        }

        private SceneChangeInfo CreateSceneChangeInfo() {
            SceneChangeInfo sceneChangeInfo = new SceneChangeInfo(sceneChangeInfoReference);
            sceneChangeInfo.cutsceneDialogue = dialogueAfterBattle;
            sceneChangeInfo.isCutscene = dialogueAfterBattle != null;
            return sceneChangeInfo;
        }
    }
}
