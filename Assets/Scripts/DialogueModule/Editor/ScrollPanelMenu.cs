using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Fog.Dialogue{

    [CustomEditor(typeof(DialogueScrollPanel))]
    public class DialogueScrollPanelEditor : UnityEditor.UI.ScrollRectEditor{
        public override void OnInspectorGUI(){
            EditorGUILayout.PropertyField(serializedObject.FindProperty("smoothScrolling"), new GUIContent("Smooth Scrolling"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scrollSpeed"), new GUIContent("Scroll Speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("marginSize"), new GUIContent("Margin Size"));
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }

    public class ScrollPanelMenu : MonoBehaviour
    {
        [MenuItem("GameObject/UI/FOG.Dialogue - ScrollPanel", false, 49)]
        static void CreateScrollPanel(MenuCommand menuCommand){
            // Create a custom game object
            GameObject panelObj = new GameObject("Dialogue Scroll Panel");
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(panelObj, "Create " + panelObj.name);
            // Cria um Canvas se ele nao existir
            Canvas canvas = FindObjectOfType<Canvas>();
            if(!canvas){
                GameObject canvasObj = new GameObject("Canvas", typeof(CanvasScaler));
                Undo.RegisterCreatedObjectUndo(canvasObj, "Create " + canvasObj.name);
                canvas = canvasObj.GetComponent<Canvas>();
                CanvasScaler canvasScaler = canvasObj.GetComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                canvasScaler.scaleFactor = 1f;
                canvasScaler.referencePixelsPerUnit = 100f;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = false;
                canvas.sortingOrder = 0;
                canvas.targetDisplay = 0;
                canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent | AdditionalCanvasShaderChannels.TexCoord1;
            }
            // Adiciona o panel como filho do canvas e seta propriedades iniciais
            panelObj.transform.parent = canvas.transform;
            panelObj.AddComponent<DialogueScrollPanel>();
            panelObj.GetComponent<CanvasRenderer>().cullTransparentMesh = false;
            panelObj.GetComponent<Mask>().showMaskGraphic = true;
            Image img = panelObj.GetComponent<Image>();
            img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            img.color = Color.white;
            img.material = null;
            img.raycastTarget = true;
            img.type = Image.Type.Sliced;
            img.fillCenter = true;
            img.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
            img.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, 150f);
            img.rectTransform.anchorMin = new Vector2(0f, 0f);
            img.rectTransform.anchorMax = new Vector2(1f, 0f);
            img.rectTransform.pivot = new Vector2(0.5f, 0f);
            img.rectTransform.position = new Vector3(img.rectTransform.position.x, 0f, img.rectTransform.position.z);
            // Adiciona o objeto de viewport como filho do panel
            GameObject viewObj = new GameObject("Viewport");
            Undo.RegisterCreatedObjectUndo(viewObj, "Create " + viewObj.name);
            viewObj.transform.parent = panelObj.transform;
            viewObj.AddComponent<Image>();
            viewObj.AddComponent<Mask>();
            img = viewObj.GetComponent<Image>();
            img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            img.color = Color.white;
            img.material = null;
            img.raycastTarget = true;
            img.type = Image.Type.Sliced;
            img.fillCenter = true;
            viewObj.GetComponent<Mask>().showMaskGraphic = false;
            viewObj.GetComponent<CanvasRenderer>().cullTransparentMesh = false;
            panelObj.GetComponent<DialogueScrollPanel>().viewport = img.rectTransform;
            img.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
            img.rectTransform.anchorMin = new Vector2(0f, 0f);
            img.rectTransform.anchorMax = new Vector2(1f, 1f);
            img.rectTransform.pivot = new Vector2(0f, 0f);
            img.rectTransform.sizeDelta = new Vector2(0f, 0f);
            img.rectTransform.position = new Vector3(0f, 0f, 0f);
            // Adiciona o objeto de content como filho do viewport
            // O content por padrao contem um TextMeshProGUI
            GameObject contentObj = new GameObject("Content");
            Undo.RegisterCreatedObjectUndo(contentObj, "Create " + contentObj.name);
            contentObj.transform.parent = viewObj.transform;
            contentObj.AddComponent<ContentSizeFitter>();
            contentObj.AddComponent<TMPro.TextMeshProUGUI>();
            contentObj.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentObj.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            TMPro.TextMeshProUGUI textMesh = contentObj.GetComponent<TMPro.TextMeshProUGUI>();
            textMesh.fontSize = 20;
            textMesh.color = Color.black;
            textMesh.alignment = TMPro.TextAlignmentOptions.TopJustified;
            textMesh.wordWrappingRatios = 0f;
            textMesh.overflowMode = TMPro.TextOverflowModes.ScrollRect;
            textMesh.margin = new Vector4(3, 20, 3, 1);
            panelObj.GetComponent<DialogueScrollPanel>().content = textMesh.rectTransform;
            textMesh.rectTransform.anchorMin = new Vector2(0f, 1f);
            textMesh.rectTransform.anchorMax = new Vector2(1f, 1f);
            textMesh.rectTransform.pivot = new Vector2(0f, 1f);
            textMesh.rectTransform.sizeDelta = new Vector2(0f, 0f);
            // Configura o DialogueHandler da cena (cria se necessario)
            DialogueHandler handler = FindObjectOfType<DialogueHandler>();
            if(!handler){
                GameObject go = new GameObject("Dialogue Handler", typeof(DialogueHandler));
                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                handler = go.GetComponent<DialogueHandler>();
            }
            handler.dialogueText = textMesh;
            handler.useTitles = true;
            handler.titleText = textMesh;
            handler.dialogueBox = panelObj.GetComponent<DialogueScrollPanel>();
            handler.useTypingEffect = true;
            handler.fillInBeforeSkip = true;
            handler.isSingleton = true;

            // Select newly created object
            panelObj.SetActive(false);
            Selection.activeObject = panelObj;
        }
    }
}
