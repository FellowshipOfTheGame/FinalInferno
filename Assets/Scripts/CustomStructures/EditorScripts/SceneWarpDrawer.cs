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
        private readonly string[] foldersToSearch = { "Assets/Scenes" };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty _sceneName = property.FindPropertyRelative("scene");
            int i = (_sceneName == null || _sceneName.stringValue == null || _sceneName.stringValue == "") ? 1 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            FindSerializedStructProperties(property);
            bool hasSerializedSceneName = !string.IsNullOrEmpty(sceneName.stringValue);
            if (sceneObj == null && hasSerializedSceneName) {
                FindSerializedSceneByName();
            }
            DrawCustomSceneField(position);
            DrawPositionFieldIfNecessary();
            EditorGUI.EndProperty();
        }
        private void FindSerializedStructProperties(SerializedProperty property) {
            sceneName = property.FindPropertyRelative("scene");
            scenePos = property.FindPropertyRelative("position");
        }

        private void FindSerializedSceneByName() {
            string[] objectsFound = FindScenesInFolders();
            if (FoundAtLeastOneScene(objectsFound)) {
                sceneObj = LoadAssetWithGUID(objectsFound[0]);
            }
        }

        private string[] FindScenesInFolders() {
            return AssetDatabase.FindAssets($"{sceneName.stringValue} t:sceneAsset", foldersToSearch);
        }

        private static bool FoundAtLeastOneScene(string[] objectsFound) {
            return objectsFound != null && objectsFound.Length > 0 && !string.IsNullOrEmpty(objectsFound[0]);
        }

        private static Object LoadAssetWithGUID(string objectGUID) {
            return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectGUID), typeof(Object));
        }

        private void DrawCustomSceneField(Rect position) {
            nameRect = new Rect(position);
            nameRect.y += 5f;
            nameRect.height = EditorGUIUtility.singleLineHeight;
            sceneObj = EditorGUI.ObjectField(nameRect, sceneObj, typeof(SceneAsset), false);
            sceneName.stringValue = (sceneObj != null) ? sceneObj.name : "";
        }

        private void DrawPositionFieldIfNecessary() {
            if (sceneObj != null) {
                posRect = EditorUtils.NewRectBelow(nameRect);
                scenePos.vector2Value = EditorGUI.Vector2Field(posRect, "Position", scenePos.vector2Value);
            } else {
                scenePos.vector2Value = Vector2.zero;
            }
        }
    }
#endif
}