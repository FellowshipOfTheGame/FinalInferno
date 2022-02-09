using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneWarp))]
    public class SceneWarpDrawer : PropertyDrawer {
        private SerializedProperty sceneName, scenePos;
        private Rect nameRect;
        private Rect posRect;
        private Object sceneObj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _sceneName = property.FindPropertyRelative("scene");
            int i = (_sceneName == null || _sceneName.stringValue == null || _sceneName.stringValue == "") ? 1 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            nameRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            posRect = new Rect(new Vector2(nameRect.position.x, nameRect.position.y + nameRect.height), new Vector2(nameRect.size.x, EditorGUIUtility.singleLineHeight));
            sceneName = property.FindPropertyRelative("scene");
            scenePos = property.FindPropertyRelative("position");

            if (sceneObj == null) {
                // Tenta achar a cena salva atualmente na pasta de cenas
                string[] objectsFound = AssetDatabase.FindAssets(sceneName.stringValue + " t:sceneAsset", new[] { "Assets/Scenes" });
                if (sceneName.stringValue != null && sceneName.stringValue != "" && objectsFound != null && objectsFound.Length > 0 && objectsFound[0] != null && objectsFound[0] != "") {
                    sceneObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectsFound[0]), typeof(Object));
                } else {
                    //Debug.Log("Não achou");
                    sceneObj = null;
                }
            }

            sceneObj = EditorGUI.ObjectField(nameRect, sceneObj, typeof(SceneAsset), false);
            sceneName.stringValue = (sceneObj != null) ? sceneObj.name : "";

            if (sceneObj != null) {
                scenePos.vector2Value = EditorGUI.Vector2Field(posRect, "Position", scenePos.vector2Value);
            } else {
                scenePos.vector2Value = Vector2.zero;
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}