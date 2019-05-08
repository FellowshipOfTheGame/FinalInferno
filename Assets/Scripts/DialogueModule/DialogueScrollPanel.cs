using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mask)), RequireComponent(typeof(Image)), RequireComponent(typeof(RectTransform))]
public class DialogueScrollPanel : ScrollRect
{
    public bool smoothScrolling;
    private bool contentIsText;
    public float scrollSpeed;
    public float marginSize;

    protected override void Reset(){
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

    protected override void Start(){
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
            StartCoroutine("ScrollingDown");
        }else
            JumpToEnd();
    }

    private IEnumerator ScrollingDown(){
        velocity = new Vector2(0f, scrollSpeed);
        if(verticalNormalizedPosition > Mathf.Epsilon)
            yield return new WaitForEndOfFrame();
        verticalNormalizedPosition = 0f;
        velocity = Vector2.zero;
    }
}
