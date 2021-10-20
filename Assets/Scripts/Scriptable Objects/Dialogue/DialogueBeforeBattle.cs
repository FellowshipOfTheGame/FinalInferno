using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "NewBattleDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueBeforeBattle")]
    public class DialogueBeforeBattle : DialogueEventTrigger
    {
        [SerializeField] private Sprite battleBG;
        [SerializeField] private AudioClip battleBGM;
        [SerializeField] private Enemy[] battleEnemies;
        [Header("Expected value = TriggerChangeScene")]
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;
        [Header("Optional")]
        [SerializeField] private DialogueFI dialogueAfterBattle;
        
        public override void AfterDialogue(){
            shouldUnlockMovement = false;

            SceneLoader.beforeSceneChange += SetFlags;
            SceneLoader.CutsceneDialogue = dialogueAfterBattle;

            FinalInferno.UI.ChangeSceneUI.isBattle = true;
            FinalInferno.UI.ChangeSceneUI.selectedDialogue = dialogueAfterBattle;
            FinalInferno.UI.ChangeSceneUI.isCutscene = (dialogueAfterBattle != null);
            FinalInferno.UI.ChangeSceneUI.battleBG = battleBG;
            FinalInferno.UI.ChangeSceneUI.battleBGM = battleBGM;
            FinalInferno.UI.ChangeSceneUI.battleEnemies = (Enemy[])battleEnemies.Clone();

            decision.Click();

            Fog.Dialogue.DialogueHandler.instance.OnDialogueStart -= AfterDialogue;
        }

        public void SetFlags(){
            base.AfterDialogue();
            SceneLoader.beforeSceneChange -= SetFlags;
        }
    }
}
