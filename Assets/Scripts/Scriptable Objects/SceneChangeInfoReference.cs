using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fog.Dialogue;

namespace FinalInferno {
    [CreateAssetMenu(fileName = "Scene Change Info Reference", menuName = "ScriptableObject/Scene Change Info Reference")]
    public class SceneChangeInfoReference : ScriptableObject {
        public ScenePicker Scene => sceneChangeInfo.scene;
        public Vector2 DestinationPosition => sceneChangeInfo.destinationPosition;
        public bool IsCutscene => sceneChangeInfo.isCutscene;
        public Dialogue CutsceneDialogue => sceneChangeInfo.cutsceneDialogue;
        public Vector2 SavePosition => sceneChangeInfo.savePosition;
        private SceneChangeInfo sceneChangeInfo = new SceneChangeInfo();

        public void SetValues(SceneChangeInfo other) {
            sceneChangeInfo.CopyValues(other);
        }

        public void Clear() {
            sceneChangeInfo.Clear();
        }

        public void LoadSceneFromMenuOrOW() {
            if (!IsCutscene) {
                SceneLoader.LoadOWSceneWithSetPosition(Scene.Name, DestinationPosition);
            } else {
                SceneLoader.LoadCustsceneWithSetPosition(Scene.Name, CutsceneDialogue, DestinationPosition, SavePosition);
            }
        }

        public void LoadSceneFromBattle() {
            if (IsCutscene) {
                SceneLoader.LoadCustsceneFromBattle(Party.Instance.currentMap, CutsceneDialogue);
            } else {
                SceneLoader.LoadOWSceneFromBattle(Party.Instance.currentMap);
            }
        }
    }
}