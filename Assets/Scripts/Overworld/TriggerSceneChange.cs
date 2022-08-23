using System.Collections.Generic;
using Fog.Dialogue;
using UnityEngine;
using FinalInferno.EventSystem;

namespace FinalInferno {
    public class TriggerSceneChange : Triggerable {
        //TO DO: Usar a struct SceneWarp aqui e atualizar o script de editor para refletir isso
        [SerializeField] private string sceneName = "Battle";
        [SerializeField] private ScenePicker scene;
        [SerializeField] private Vector2 positionOnLoad = new Vector2(0, 0);
        [SerializeField] private Vector2 saveGamePosition = new Vector2(0, 0);
        [SerializeField] private bool isCutscene = false;
        [SerializeField] private List<DialogueEntry> dialogues = new List<DialogueEntry>();
        [SerializeField] SceneChangeInfoReference sceneChangeInfoReference;
        [SerializeField] private EventFI startSceneChangeAnimation;
        private SceneChangeInfo sceneChangeInfo;

        private void Awake() {
            SaveFixedSceneChangeParameters();
        }

        private void SaveFixedSceneChangeParameters() {
            sceneChangeInfo = new SceneChangeInfo();
            sceneChangeInfo.scene.CopyValues(scene);
            sceneChangeInfo.destinationPosition = positionOnLoad;
            sceneChangeInfo.savePosition = saveGamePosition;
            sceneChangeInfo.isCutscene = isCutscene;
        }

        protected override void TriggerAction(Agent agent) {
            if (string.IsNullOrEmpty(sceneName)) {
                return;
            }
            CharacterOW.PartyCanMove = false;
            Dialogue selectedDialogue = null;
            if (isCutscene) {
                selectedDialogue = DialogueEntry.GetLastUnlockedDialogue(dialogues);
            }
            ChangeSceneWithDialogue(selectedDialogue);
        }

        private void ChangeSceneWithDialogue(Dialogue selectedDialogue) {
            sceneChangeInfo.cutsceneDialogue = selectedDialogue;
            sceneChangeInfoReference.SetValues(sceneChangeInfo);
            startSceneChangeAnimation.Raise();
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
