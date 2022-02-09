using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScenePicker))]
    public class ScenePickerEditor : PropertyDrawer {
        private SerializedProperty sceneName;
        private SerializedProperty assetPath;
        private SerializedProperty guid;
        private Rect rect;
        private Object sceneObj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            rect = new Rect(position.x, position.y, position.size.x, EditorGUIUtility.singleLineHeight);
            sceneName = property.FindPropertyRelative("sceneName");
            assetPath = property.FindPropertyRelative("assetPath");
            guid = property.FindPropertyRelative("guid");

            if (sceneObj == null && assetPath.stringValue != "") {
                sceneObj = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath.stringValue);
            } else if (assetPath.stringValue == "") {
                sceneObj = null;
            }

            sceneObj = EditorGUI.ObjectField(rect, sceneObj, typeof(SceneAsset), false);

            if (sceneObj != null) {
                sceneName.stringValue = sceneObj.name;
                assetPath.stringValue = AssetDatabase.GetAssetPath(sceneObj.GetInstanceID());
                guid.stringValue = AssetDatabase.AssetPathToGUID(assetPath.stringValue);
            } else {
                sceneName.stringValue = "";
                assetPath.stringValue = "";
                guid.stringValue = "";
            }

            EditorGUI.EndProperty();
        }
    }

#endif

}