using System.Collections;
using UnityEngine;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "EndChapterDialogue", menuName = "ScriptableObject/DialogueSystem/FinalInferno/DialogueEndChapter")]
    public class DialogueEndChapter : Fog.Dialogue.Dialogue {
        public override void BeforeDialogue() { }
        public override void AfterDialogue() {
            Fog.Dialogue.DialogueHandler.instance.StartCoroutine(WaitToGoBack());
        }

        public IEnumerator WaitToGoBack() {
            yield return new WaitForSecondsRealtime(1.5f);
            BackToMainMenu();
        }

        public void BackToMainMenu() {
            SceneLoader.LoadMainMenu();
        }
    }

}
