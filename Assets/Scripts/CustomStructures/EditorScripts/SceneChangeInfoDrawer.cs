using UnityEditor;
using UnityEngine;

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneChangeInfo))]
    public class SceneChangeInfoDrawer : PropertyDrawer {
        private SerializedProperty scene;
        private SerializedProperty destinationPosition;
        private SerializedProperty isCutscene;
        private SerializedProperty cutsceneDialogue;
        private SerializedProperty savePosition;
        private Rect rect;
        private bool foldoutOpened = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!foldoutOpened)
                return EditorGUIUtility.singleLineHeight;
            isCutscene = property.FindPropertyRelative("isCutscene");
            int nLines = isCutscene.boolValue ? 6 : 4;
            return nLines * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            position.height = EditorGUIUtility.singleLineHeight;
            foldoutOpened = EditorGUI.Foldout(position, foldoutOpened, label);
            if (foldoutOpened) {
                FindSerializedStructProperties(property);
                position = DrawSceneChangeParameters(position);
                DrawCutsceneIfNecessary(position);
            }
            EditorGUI.EndProperty();
        }

        private void FindSerializedStructProperties(SerializedProperty property) {
            scene = property.FindPropertyRelative("scene");
            destinationPosition = property.FindPropertyRelative("destinationPosition");
            isCutscene = property.FindPropertyRelative("isCutscene");
            cutsceneDialogue = property.FindPropertyRelative("cutsceneDialogue");
            savePosition = property.FindPropertyRelative("savePosition");
        }

        private Rect DrawSceneChangeParameters(Rect position) {
            position = EditorUtils.NewRectBelow(position);
            EditorGUI.PropertyField(position, scene);
            position = EditorUtils.NewRectBelow(position);
            EditorGUI.PropertyField(position, destinationPosition);
            position = EditorUtils.NewRectBelow(position);
            EditorGUI.PropertyField(position, isCutscene);
            return position;
        }

        private void DrawCutsceneIfNecessary(Rect position) {
            if (!isCutscene.boolValue)
                return;
            position = EditorUtils.NewRectBelow(position);
            EditorGUI.PropertyField(position, cutsceneDialogue);
            position = EditorUtils.NewRectBelow(position);
            EditorGUI.PropertyField(position, savePosition);
        }
    }
#endif
}