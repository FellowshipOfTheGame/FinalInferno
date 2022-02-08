using System.Collections.Generic;
using UnityEditor;
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
            if (sceneName != null && sceneName != "") {
                CharacterOW.PartyCanMove = false;

                Fog.Dialogue.Dialogue selectedDialogue = null;
                if (isCutscene) {
                    foreach (DialogueEntry entry in dialogues) {
                        //Debug.Log("Checking dialogue: " + entry.dialogue + " with quest " + entry.quest + " and event " + entry.eventFlag);
                        if (entry.quest.GetFlag(entry.eventFlag)) {
                            selectedDialogue = entry.dialogue;
                        } else {
                            //Debug.Log("Event " + entry.eventFlag + " deu false");
                            break;
                        }
                    }
                }
                //Debug.Log("Loading scene: " + sceneName + "; dialogue = " + selectedDialogue);
                FinalInferno.UI.ChangeSceneUI.sceneName = sceneName;
                FinalInferno.UI.ChangeSceneUI.positionOnLoad = positionOnLoad;
                FinalInferno.UI.ChangeSceneUI.savePosition = saveGamePosition;
                FinalInferno.UI.ChangeSceneUI.isCutscene = isCutscene;
                FinalInferno.UI.ChangeSceneUI.selectedDialogue = selectedDialogue;

                decision.Click();
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(positionOnLoad.x, positionOnLoad.y, 0), new Vector3(1, 1, 1));
            if (isCutscene) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(saveGamePosition.x, saveGamePosition.y, 0), new Vector3(1, 1, 1));
            }
        }
    }
#if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomEditor(typeof(TriggerSceneChange))]
    public class TriggerSceneChangeEditor : Editor {
        private SerializedProperty sceneName;
        private Object sceneObj;

        public void OnEnable() {
            sceneName = serializedObject.FindProperty("sceneName");
            //Debug.Log("Procurando " + sceneName.stringValue +  "...");
            string[] objectsFound = AssetDatabase.FindAssets(sceneName.stringValue + " t:sceneAsset", new[] { "Assets/Scenes" });
            if (sceneName.stringValue != "" && objectsFound != null && objectsFound.Length > 0 && objectsFound[0] != null && objectsFound[0] != "") {
                sceneObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectsFound[0]), typeof(Object));
            } else {
                //Debug.Log("Não achou");
                sceneObj = null;
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            sceneObj = EditorGUILayout.ObjectField(sceneObj, typeof(SceneAsset), false);
            if (sceneObj != null) {
                sceneName.stringValue = sceneObj.name;
            } else {
                sceneName.stringValue = "";
            }

            if (sceneObj != null) {
                SerializedProperty positionOnLoad = serializedObject.FindProperty("positionOnLoad");
                SerializedProperty isCutscene = serializedObject.FindProperty("isCutscene");
                SerializedProperty decision = serializedObject.FindProperty("decision");
                EditorGUILayout.PropertyField(positionOnLoad);
                EditorGUILayout.PropertyField(isCutscene);
                EditorGUILayout.PropertyField(decision);
                if (isCutscene.boolValue) {
                    SerializedProperty saveGamePosition = serializedObject.FindProperty("saveGamePosition");
                    SerializedProperty dialogues = serializedObject.FindProperty("dialogues");
                    EditorGUILayout.PropertyField(saveGamePosition);
                    EditorGUILayout.PropertyField(dialogues, includeChildren: true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
