using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FinalInferno.Input {
#if UNITY_EDITOR
    [CustomEditor(typeof(InputDependentImage))]
    [CanEditMultipleObjects]
    public class InputDependentImageEditor : UnityEditor.UI.ImageEditor {
        private SerializedProperty inputActions;
        private SerializedProperty controlSchemesNames;
        private SerializedProperty controlSchemesImages;

        protected override void OnEnable() {
            base.OnEnable();
            FindSerializedProperties();
        }

        private void FindSerializedProperties() {
            inputActions = serializedObject.FindProperty("inputActions");
            controlSchemesNames = serializedObject.FindProperty("controlSchemesNames");
            controlSchemesImages = serializedObject.FindProperty("controlSchemesImages");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DisplayActionAssetPicker();
            DisplayImageConfigIfNeeded();
            serializedObject.ApplyModifiedProperties();
            DisplayRegularImageEditor();
        }

        private void DisplayActionAssetPicker() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Input Info:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(inputActions);
        }

        private void DisplayImageConfigIfNeeded() {
            if (inputActions.objectReferenceValue != null) {
                EditorGUI.indentLevel++;
                InputActionAsset inputActionAsset = inputActions.objectReferenceValue as InputActionAsset;
                string[] schemeNames = GetSchemeNames(inputActionAsset);
                controlSchemesNames.ClearArray();
                for (int i = 0; i < schemeNames.Length; i++) {
                    EditorGUILayout.BeginHorizontal();
                    SaveAndDisplaySchemeName(schemeNames, i);
                    DisplaySpritePicker(i);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }

        private void SaveAndDisplaySchemeName(string[] schemeNames, int index) {
            controlSchemesNames.InsertArrayElementAtIndex(index);
            controlSchemesNames.GetArrayElementAtIndex(index).stringValue = schemeNames[index];
            EditorGUILayout.PrefixLabel(schemeNames[index]);
        }

        private void DisplaySpritePicker(int index) {
            if (index >= controlSchemesImages.arraySize) {
                controlSchemesImages.InsertArrayElementAtIndex(index);
            }
            SerializedProperty imageProperty = controlSchemesImages.GetArrayElementAtIndex(index);
            imageProperty.objectReferenceValue = EditorGUILayout.ObjectField(imageProperty.objectReferenceValue, typeof(Sprite), false);
        }

        private static string[] GetSchemeNames(InputActionAsset inputActionAsset) {
            return Array.ConvertAll<InputControlScheme, string>(inputActionAsset.controlSchemes.ToArray(), map => map.name);
        }

        private void DisplayRegularImageEditor() {
            EditorGUILayout.LabelField("Regular Image Info:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }

        #region Conversion
        // Baseado no código do pacote SoftMaskForUGUI by mob-sakai
        [MenuItem("CONTEXT/Image/Convert to Input Dependent Image", true)]
        private static bool _ConvertToInputDependentImage(MenuCommand command) {
            return command.context && command.context.GetType() != typeof(InputDependentImage);
        }

        [MenuItem("CONTEXT/Image/Convert to Input Dependent Image", false)]
        private static void ConvertToInputDependentImage(MenuCommand command) {
            ConvertTo<InputDependentImage>(command.context);
        }

        [MenuItem("CONTEXT/Image/Convert to Image", true)]
        private static bool _ConvertToImage(MenuCommand command) {
            return command.context && command.context.GetType() != typeof(Image);
        }

        [MenuItem("CONTEXT/Image/Convert to Image", false)]
        private static void ConvertToImage(MenuCommand command) {
            ConvertTo<Image>(command.context);
        }

        private static void ConvertTo<T>(UnityEngine.Object context) where T : MonoBehaviour {
            MonoBehaviour target = context as MonoBehaviour;
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            bool oldEnable = target.enabled;
            target.enabled = false;

            // Find MonoScript of the specified component.
            foreach (string scriptGUID in AssetDatabase.FindAssets("t:MonoScript")) {
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(scriptGUID));
                if (script.GetClass() != typeof(T)) {
                    continue;
                }

                // Set 'm_Script' to convert.
                serializedObject.FindProperty("m_Script").objectReferenceValue = script;
                serializedObject.ApplyModifiedProperties();
                break;
            }

            (serializedObject.targetObject as MonoBehaviour).enabled = oldEnable;
        }
        #endregion
    }

#endif
}
