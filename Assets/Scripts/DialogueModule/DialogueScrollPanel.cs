using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fog.Dialogue{
    [RequireComponent(typeof(Mask)), RequireComponent(typeof(Image)), RequireComponent(typeof(RectTransform))]
    public class DialogueScrollPanel : ScrollRect
    {
        public bool smoothScrolling;
        private bool contentIsText;
        [Fog.Editor.HideInInspectorIfNot(nameof(smoothScrolling))]
        public float scrollSpeed;
        public float marginSize; // To do

        protected void Reset(){
            smoothScrolling = false;
            scrollSpeed = 10f;
            marginSize = 0f;
            content = null;
            horizontal = false;
            vertical = true;
            movementType = MovementType.Clamped;
            inertia = false;
            scrollSensitivity = 1;
            viewport = null;
            horizontalScrollbar = null;
            horizontalScrollbarSpacing = 1f;
            horizontalScrollbarVisibility = ScrollbarVisibility.AutoHideAndExpandViewport;
            verticalScrollbar = null;
            verticalScrollbarSpacing = 1f;
            verticalScrollbarVisibility = ScrollbarVisibility.AutoHideAndExpandViewport;
            onValueChanged = null;
        }

        protected void Start(){
            base.Start();
            if(content){
                contentIsText = (content.GetComponent<TMPro.TextMeshProUGUI>() != null) || (content.GetComponent<Text>() != null);
            }else
                contentIsText = false;
        }

        public void JumpToEnd(){
            StopCoroutine("ScrollingDown");
            Canvas.ForceUpdateCanvases();
            verticalNormalizedPosition = 0f;
        }

        public void ScrollToEnd(){
            if(smoothScrolling){
                StopCoroutine("ScrollingDown");
                StartCoroutine("ScrollingDown");
            }else
                JumpToEnd();
        }

        private IEnumerator ScrollingDown(){
            yield return new WaitForEndOfFrame();
            while(verticalNormalizedPosition > Mathf.Epsilon){
                Canvas.ForceUpdateCanvases();
                verticalNormalizedPosition -= (Time.deltaTime * scrollSpeed * 10)/(content.rect.height);
                yield return new WaitForEndOfFrame();
            }
            verticalNormalizedPosition = 0f;
            velocity = Vector2.zero;
        }
    }
}
