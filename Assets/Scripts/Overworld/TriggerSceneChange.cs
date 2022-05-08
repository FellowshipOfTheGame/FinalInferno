using System.Collections.Generic;
using Fog.Dialogue;
using UnityEngine;

namespace FinalInferno {
    public class TriggerSceneChange : Triggerable {
        //TO DO: Usar a struct SceneWarp aqui e atualizar o script de editor para refletir isso
        [SerializeField] private string sceneName = "Battle";
        [SerializeField] private Vector2 positionOnLoad = new Vector2(0, 0);
        [SerializeField] private Vector2 saveGamePosition = new Vector2(0, 0);
        [SerializeField] private bool isCutscene = false;
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        [SerializeField] private FinalInferno.UI.FSM.ButtonClickDecision decision;

        protected override void TriggerAction(Fog.Dialogue.Agent agent) {
            if (string.IsNullOrEmpty(sceneName)) {
                return;
            }
            CharacterOW.PartyCanMove = false;
            Fog.Dialogue.Dialogue selectedDialogue = null;
            if (isCutscene) {
                selectedDialogue = DialogueEntry.GetLastUnlockedDialogue(dialogues);
            }
            ChangeSceneWithDialogue(selectedDialogue);
        }

        private void ChangeSceneWithDialogue(Dialogue selectedDialogue) {
            FinalInferno.UI.ChangeSceneUI.sceneName = sceneName;
            FinalInferno.UI.ChangeSceneUI.positionOnLoad = positionOnLoad;
            FinalInferno.UI.ChangeSceneUI.savePosition = saveGamePosition;
            FinalInferno.UI.ChangeSceneUI.isCutscene = isCutscene;
            FinalInferno.UI.ChangeSceneUI.selectedDialogue = selectedDialogue;
            decision.Click();
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(positionOnLoad.x, positionOnLoad.y, 0), Vector3.one);
            if (isCutscene) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(saveGamePosition.x, saveGamePosition.y, 0), Vector3.one);
            }
        }
    }
}
