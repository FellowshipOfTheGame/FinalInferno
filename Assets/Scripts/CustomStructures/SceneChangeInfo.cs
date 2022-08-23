using UnityEngine;
using Fog.Dialogue;

namespace FinalInferno {
    [System.Serializable]
    public struct SceneChangeInfo {
        public ScenePicker scene;
        public Vector2 destinationPosition;
        public bool isCutscene;
        public Dialogue cutsceneDialogue;
        public Vector2 savePosition;

        public SceneChangeInfo(ScenePicker scene, Vector2 destinationPosition) {
            this.scene = new ScenePicker(scene);
            this.destinationPosition = destinationPosition;
            isCutscene = false;
            cutsceneDialogue = null;
            savePosition = Vector2.zero;
        }

        public SceneChangeInfo(ScenePicker scene, Vector2 destinationPosition, bool isCutscene, Dialogue dialogue, Vector2 savePosition) {
            this.scene = new ScenePicker(scene);
            this.destinationPosition = destinationPosition;
            this.isCutscene = isCutscene;
            cutsceneDialogue = dialogue;
            this.savePosition = savePosition;
        }

        public SceneChangeInfo(SceneChangeInfoReference reference) {
            scene = new ScenePicker(reference.Scene);
            destinationPosition = reference.DestinationPosition;
            isCutscene = reference.IsCutscene;
            cutsceneDialogue = reference.CutsceneDialogue;
            savePosition = reference.SavePosition;
        }

        public void CopyValues(SceneChangeInfo other) {
            if (scene == null)
                scene = new ScenePicker(other.scene);
            else
                scene.CopyValues(other.scene);
            destinationPosition = other.destinationPosition;
            isCutscene = other.isCutscene;
            cutsceneDialogue = other.cutsceneDialogue;
            savePosition = other.savePosition;
        }

        public void Clear() {
            scene = new ScenePicker();
            destinationPosition = Vector2.zero;
            isCutscene = false;
            cutsceneDialogue = null;
            savePosition = Vector2.zero;
        }
    }
}
