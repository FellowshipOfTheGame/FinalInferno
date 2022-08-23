using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "NewBattleDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueBeforeBattle")]
    public class DialogueBeforeBattle : DialogueEventTrigger, IUpdatableScript {
        [SerializeField] private Sprite battleBG;
        [SerializeField] private AudioClip battleBGM;
        [SerializeField] private Enemy[] battleEnemies;
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

        public void UpdateThisObject() {
            battleInfo = new BattleInfo(battleEnemies, battleBG, battleBGM);
            string guid = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(BattleInfoReference)}")[0];
            battleInfoReference = UnityEditor.AssetDatabase.LoadAssetAtPath<BattleInfoReference>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
            guid = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(SceneChangeInfoReference)}")[0];
            sceneChangeInfoReference = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneChangeInfoReference>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
            guid = UnityEditor.AssetDatabase.FindAssets($"Start Scene Change t:{typeof(EventFI)}")[0];
            startSceneChangeAnimation = UnityEditor.AssetDatabase.LoadAssetAtPath<EventFI>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
            guid = UnityEditor.AssetDatabase.FindAssets($"Is Loading Battle t:{typeof(BoolVariable)}")[0];
            isLoadingBattle = UnityEditor.AssetDatabase.LoadAssetAtPath<BoolVariable>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}
