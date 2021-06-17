using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.InputSystem;

namespace FinalInferno.Input{
    [CustomEditor(typeof(InputDependentImage))]
    public class InputDependentImageEditor : UnityEditor.UI.ImageEditor {
        SerializedProperty inputActions;
        SerializedProperty controlSchemesNames;
        SerializedProperty controlSchemesImages;

        protected override void OnEnable(){
            base.OnEnable();
            inputActions = serializedObject.FindProperty("inputActions");
            controlSchemesNames = serializedObject.FindProperty("controlSchemesNames");
            controlSchemesImages = serializedObject.FindProperty("controlSchemesImages");
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();

            EditorGUILayout.LabelField("Input Info:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(inputActions);
            if(inputActions.objectReferenceValue != null){
                EditorGUI.indentLevel++;
                InputActionAsset inputActionAsset = inputActions.objectReferenceValue as InputActionAsset;
                string[] schemeNames = Array.ConvertAll<InputControlScheme, string>(inputActionAsset.controlSchemes.ToArray(), map => map.name);
                controlSchemesNames.ClearArray();
                for(int i = 0; i < schemeNames.Length; i++){
                    EditorGUILayout.BeginHorizontal();

                    controlSchemesNames.InsertArrayElementAtIndex(i);
                    controlSchemesNames.GetArrayElementAtIndex(i).stringValue = schemeNames[i];
                    EditorGUILayout.PrefixLabel(schemeNames[i]);

                    if(i >= controlSchemesImages.arraySize){
                        controlSchemesImages.InsertArrayElementAtIndex(i);
                    }
                    SerializedProperty imageProperty = controlSchemesImages.GetArrayElementAtIndex(i);
                    imageProperty.objectReferenceValue = EditorGUILayout.ObjectField(imageProperty.objectReferenceValue, typeof(Sprite), false);

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Regular Image Info:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }

        // Conversion
        // Baseado no código do pacote SoftMaskForUGUI by mob-sakai
        [MenuItem("CONTEXT/Image/Convert to Input Dependent Image", true)]
        private static bool _ConvertToInputDependentImage(MenuCommand command)
        {
            return command.context && command.context.GetType() != typeof(InputDependentImage);
        }

        [MenuItem("CONTEXT/Image/Convert to Input Dependent Image", false)]
        private static void ConvertToInputDependentImage(MenuCommand command)
        {
            ConvertTo<InputDependentImage>(command.context);
        }

        [MenuItem("CONTEXT/Image/Convert to Image", true)]
        private static bool _ConvertToImage(MenuCommand command)
        {
            return command.context && command.context.GetType() != typeof(Image);
        }

        [MenuItem("CONTEXT/Image/Convert to Image", false)]
        private static void ConvertToImage(MenuCommand command)
        {
            ConvertTo<Image>(command.context);
        }

        private static void ConvertTo<T>(UnityEngine.Object context) where T : MonoBehaviour
        {
            var target = context as MonoBehaviour;
            var serializedObject = new SerializedObject(target);
            serializedObject.Update();

            var oldEnable = target.enabled;
            target.enabled = false;

            // Find MonoScript of the specified component.
            foreach(string scriptGUID in AssetDatabase.FindAssets("t:MonoScript")){
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(scriptGUID));
                if (script.GetClass() != typeof(T))
                    continue;

                // Set 'm_Script' to convert.
                serializedObject.FindProperty("m_Script").objectReferenceValue = script;
                serializedObject.ApplyModifiedProperties();
                break;
            }

            (serializedObject.targetObject as MonoBehaviour).enabled = oldEnable;
        }
    }
}
